using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Global manager responsible for controlling minigame lifecycle, including activation, deactivation, and system-wide state switching.
/// </summary>
///
/// Responsibilities:
/// - Detect player interaction to start minigames
/// - Switch between main gameplay and minigame cameras
/// - Enable/disable UI and world effects (volume, shake, cursor)
/// - Coordinate with order and validation systems
/// - Handle minigame completion results
///
/// Flow:
/// Player interaction → StartMinigame → minigame gameplay loop → EndMinigame → restore world state

public class MinigamesManager : InstanceBaseClass<MinigamesManager>
{
    /// <summary>
    /// Reference to the main gameplay camera used during normal play sessions.
    /// </summary>
    [SerializeField] private GameObject mainCamera;

    /// <summary>
    /// Reference to the dedicated minigame camera used during minigame sessions.
    /// </summary>
    [SerializeField] private GameObject minigameCamera;

    // References to camera shake components for both main gameplay and minigame contexts
    [SerializeField] private Shaker mainShaker;
    [SerializeField] private Shaker minigameShaker;

    // References to post-processing volumes for visual effects during main gameplay and minigame sessions
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private GameObject minigameVolume;

    // Reference to the UI component responsible for displaying minigame time feedback to the player
    [SerializeField] private TimeIndicator timeIndicator;

    /// <summary>
    /// Indicates whether a minigame session is currently active.
    /// </summary>
    [field: SerializeField] public bool IsInMinigame { get; private set; }

    private void Update()
    {
        // Safety check to prevent starting minigame if global conditions are not met
        if (!GameManager.Instance.CanStartMinigame) return;

        // Safety check to ensure minigame is not started if there is no active order or if the current order is already marked as ready
        if (!OrderManager.Instance.HasActiveOrder || OrderManager.Instance.IsOrderReady) return;

        // Safety check to ensure player is within a valid trigger zone that can start a minigame session
        if (!NPC_OrderController.Instance.CurrentZone) return;

        // Starts the minigame session when the player presses the interaction button and the current zone is not already completed
        if (InputManager.Instance.InteractionAction.WasPressedThisFrame() && !NPC_OrderController.Instance.CurrentZone.IsCompleted)
        {
            StartMinigame();
        }
    }

    /// <summary>
    /// Activates minigame mode and switches all related systems.
    /// </summary>
    public void StartMinigame()
    {
        // Set global state to indicate minigame mode is active
        IsInMinigame = true;

        // Switch cameras
        minigameCamera.SetActive(true);
        mainCamera.SetActive(false);

        // Switch visual post-processing
        globalVolume.SetActive(false);
        minigameVolume.SetActive(true);

        // Adjust camera shake depending on current shift conditions
        if (ShiftManager.Instance.IsPeakHour)
        {
            mainShaker.StopShaking();
            minigameShaker.Shake();
        }

        // Switch input system to minigame context
        InputManager.Instance.OnMinigameStarted();
        InputManager.Instance.OpenBookAction.Disable();

        // Initialize UI 
        timeIndicator.Initialize();

        // Start current zone minigame
        NPC_OrderController.Instance.CurrentZone.StartMinigame();

        // Show cursor for interaction
        CursorLogic.Instance.SetVisible();
    }

    /// <summary>
    /// Ends the minigame session and restores normal gameplay state.
    /// </summary>
    public void EndMinigame(bool completed)
    {
        // Hide cursor after minigame ends
        CursorLogic.Instance.SetInvisible();

        // Restore visual volumes and cameras to main gameplay state
        minigameVolume.SetActive(false);
        mainCamera.SetActive(true);

        globalVolume.SetActive(true);
        minigameCamera.SetActive(false);

        // Restore camera shake behavior based on current shift conditions
        if (ShiftManager.Instance.IsPeakHour)
        {
            minigameShaker.StopShaking();
            mainShaker.Shake();
        }

        // Restore input system state
        InputManager.Instance.OnMinigameFinished();
        InputManager.Instance.OpenBookAction.Enable();

        // Update global state to indicate minigame mode is no longer active
        IsInMinigame = false;

        // Calculate and set order result if the minigame was completed successfully, then mark the order as ready and update the current zone state to completed
        if (completed)
        {
            OrderValidator.Instance.SetOrderResult();
            OrderManager.Instance.MarkOrderAsReady();
            NPC_OrderController.Instance.CurrentZone.SetCompleted(true);
        }
    }
}
