using System;
using UnityEngine;

/// <summary>
/// Base implementation for a single interactive minigame step. Handles execution lifecycle, player input processing, and interaction flow with scene objects.
/// </summary>
///
/// Responsibilities:
/// - Activate/deactivate step logic
/// - Process player raycast interactions
/// - Notify systems about step start/completion
/// - Provide extensible hooks for derived step behaviors
///
/// Architecture:
/// - Implements IExecutable for state machine integration
/// - Uses inheritance for specialized gameplay steps
/// - Communicates through events and interfaces to reduce coupling

public class BaseStepManager : MonoBehaviour, IExecutable
{
    // Invoked when the step becomes active
    public Action OnStepStarted;

    // Invoked when the step finishes execution
    public Action OnStepCompleted;

    /// <summary>
    /// Root object containing all visuals and gameplay elements required for this step.
    /// </summary>
    [field: SerializeField] public GameObject StageSetUp { get; protected set; }

    /// <summary>
    /// Reference used to notify the state system when this step is completed.
    /// </summary>
    public IStateHandler StateHandler { get; set; }

    #region UnityMethods

    protected virtual void OnEnable()
    {
        // Subscribes to player mouse input when the step becomes active.
        InputManager.Instance.MouseAction.performed += Check;
    }

    protected virtual void OnDisable()
    {
        // Cleans up subscriptions and disables setup objects when the step is no longer active.
        InputManager.Instance.MouseAction.performed -= Check;

        // Hide stage-specific objects after being disabled to prevent unintended interactions or visuals.
        StageSetUp.SetActive(false);
    }

    #endregion

    /// <summary>
    /// Processes player interaction input using raycasting. Detects interactable objects under the cursor.
    /// </summary>
    protected virtual void Check(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Try to retrieve interactable object under mouse cursor
        InteractableObject raycastResult = Raycaster.GetRaycastResult(RaycastUtility.GetRay());

        // Trigger interaction callback if object exists
        if (raycastResult)
            raycastResult.OnRaycast();
    }

    /// <summary>
    /// Hook called before an object is used/interacted with. Intended for override in derived step implementations.
    /// </summary>
    public virtual void UpdateBeforeUsed(MonoBehaviour queryObject = null)
    {
    }

    /// <summary>
    /// Hook called after interaction logic is completed. By default, marks step requirements as completed.
    /// </summary>
    public virtual void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        SetCompleted();
    }

    /// <summary>
    /// Notifies the switch button system that this step has satisfied its completion requirements, so the player can progress to the next stage. 
    /// </summary>
    protected virtual void SetCompleted()
    {
        SwitchBTNLogic.Instance.RequirementsCompleted(this);
    }

    /// <summary>
    /// Returns whether interaction with the queried object is allowed. Intended for custom validation logic in derived classes.
    /// </summary>
    public virtual bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return false;
    }

    #region Execution

    /// <summary>
    /// Activates this step and initializes its gameplay logic.
    /// </summary>
    public virtual void Execute()
    {
        // Enable stage visuals/setup
        StageSetUp.SetActive(true);

        // Enable step object itself
        gameObject.SetActive(true);

        // Notify listeners
        OnStepStarted?.Invoke();
    }

    /// <summary>
    /// Ends execution of this step and performs cleanup.
    /// </summary>
    public virtual void EndExecution()
    {
        // Notify listeners about completion
        OnStepCompleted?.Invoke();

        // Disable step object after completion
        gameObject.SetActive(false);
    }

    #endregion
}