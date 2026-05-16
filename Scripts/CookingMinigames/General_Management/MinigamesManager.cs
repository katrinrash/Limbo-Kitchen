using UnityEngine;
using UnityEngine.InputSystem;

public class MinigamesManager : InstanceBaseClass<MinigamesManager>
{
    [SerializeField] private Camera mainCamera; // 
    [SerializeField] private Camera minigameCamera; //
    [SerializeField] private Shaker mainShaker;
    [SerializeField] private Shaker minigameShaker;
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private GameObject minigameVolume;
    [SerializeField] private TimeIndicator timeIndicator;

    [field: SerializeField] public bool IsInMinigame { get; private set; } // update
    void Update()
    {
        if (!GameManager.Instance.CanStartMinigame) return;

        if (!OrderManager.Instance.HasActiveOrder || OrderManager.Instance.IsOrderReady) return;
        else if (!NPC_OrderController.Instance.CurrentZone) return;

        if (InputManager.Instance.InteractionAction.WasPressedThisFrame() && !NPC_OrderController.Instance.CurrentZone.IsCompleted)
        {
            StartMinigame();
            //FaderManager.Instance.FadeIn();
            //Invoke(nameof(StartMinigame), FaderManager.Instance.FadeDuration);
        }
    }

    public void StartMinigame()
    {
        IsInMinigame = true;
        minigameCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
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
        mainCamera.gameObject.SetActive(true);
        globalVolume.SetActive(true);
        minigameCamera.gameObject.SetActive(false);

        if (ShiftManager.Instance.IsPeakHour)
        {
            minigameShaker.StopShaking();
            mainShaker.Shake();
        }

        InputManager.Instance.OnMinigameFinished();
        InputManager.Instance.OpenBookAction.Enable();
        //FaderManager.Instance.FadeOut();
        IsInMinigame = false;

        if (completed)
        {
            OrderValidator.Instance.SetOrderResult();
            OrderManager.Instance.MarkOrderAsReady();
            NPC_OrderController.Instance.CurrentZone.SetCompleted(true);
        }
    }
}
