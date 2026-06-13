using UnityEngine;
using UnityEngine.InputSystem;

// Global manager responsible for controlling minigame lifecycle, including activation, deactivation, and system-wide state switching.
//
// Responsibilities:
// - Detect player interaction to start minigames
// - Switch between main gameplay and minigame cameras
// - Enable/disable UI and world effects (volume, shake, cursor)
// - Coordinate with order and validation systems
// - Handle minigame completion results
//
// Flow:
// Player interaction → StartMinigame → minigame gameplay loop → EndMinigame → restore world state

public class MinigamesManager : InstanceBaseClass<MinigamesManager>
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject minigameCamera;

    [SerializeField] private Shaker mainShaker;
    [SerializeField] private Shaker minigameShaker;

    [SerializeField] private GameObject globalVolume;
    [SerializeField] private GameObject minigameVolume;

    [SerializeField] private TimeIndicator timeIndicator;

    [field: SerializeField] public bool IsInMinigame { get; private set; }

    private void Update()
    {
        if (!GameManager.Instance.CanStartMinigame) return;

        if (!OrderManager.Instance.HasActiveOrder || OrderManager.Instance.IsOrderReady) return;

        if (!NPC_OrderController.Instance.CurrentZone) return;

        if (InputManager.Instance.InteractionAction.WasPressedThisFrame() && !NPC_OrderController.Instance.CurrentZone.IsCompleted)
        {
            StartMinigame();
        }
    }

    public void StartMinigame()
    {
        IsInMinigame = true;

        minigameCamera.SetActive(true);
        mainCamera.SetActive(false);

        globalVolume.SetActive(false);
        minigameVolume.SetActive(true);

        if (ShiftManager.Instance.IsPeakHour)
        {
            mainShaker.StopShaking();
            minigameShaker.Shake();
        }

        InputManager.Instance.OnMinigameStarted();
        InputManager.Instance.OpenBookAction.Disable();

        timeIndicator.Initialize();

        NPC_OrderController.Instance.CurrentZone.StartMinigame();

        CursorLogic.Instance.SetVisible();
    }

    public void EndMinigame(bool completed)
    {
        CursorLogic.Instance.SetInvisible();

        minigameVolume.SetActive(false);
        mainCamera.SetActive(true);

        globalVolume.SetActive(true);
        minigameCamera.SetActive(false);

        if (ShiftManager.Instance.IsPeakHour)
        {
            minigameShaker.StopShaking();
            mainShaker.Shake();
        }

        InputManager.Instance.OnMinigameFinished();
        InputManager.Instance.OpenBookAction.Enable();

        IsInMinigame = false;

        if (completed)
        {
            OrderValidator.Instance.SetOrderResult();
            OrderManager.Instance.MarkOrderAsReady();
            NPC_OrderController.Instance.CurrentZone.SetCompleted(true);
        }
    }
}
