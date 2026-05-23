using UnityEngine;

/// <summary>
/// Base controller for spoon interactable objects. Handles shared logic to reduce code duplication across different spoon types.
/// </summary>

public class BaseFillingSpoonController : InteractableObject
{
    // Reference to the sprite renderer used to update spoon visuals
    [SerializeField] protected SpriteRenderer spoonSpriteRenderer;

    // Step manager controlling current gameplay stage
    [SerializeField] protected BaseStepManager stepManager;

    // An array of sprites representing different spoon states
    [SerializeField] protected Sprite[] spoonSprites;

    // Tag used to validate target interaction
    [SerializeField] protected string targetTag;

    /// <summary>
    /// Indicates whether the spoon currently contains anything.
    /// </summary>
    public bool IsFilled { get; protected set; } = false;

    // Initial position of an object
    protected Vector3 _defaultPosition;

    #region UnityMethods

    private void Awake()
    {
        // Cache the default position
        _defaultPosition = transform.position;

        // Subscribe to step completion event to reset state when the step is completed
        stepManager.OnStepCompleted += Disable;

        // Disabled until player interaction begins
        enabled = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe from step completion event
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    /// <summary>
    /// Activates spoon interaction when hit by a raycast.
    /// </summary>
    public override void OnRaycast()
    {
        enabled = true;
    }

    /// <summary>
    /// Validate current spoon object interaction. Intended to be overridden by specific spoon types to implement custom validation logic
    /// </summary>
    protected virtual void Check() { }

    /// <summary>
    /// Updates spoon after successful interaction.
    /// </summary>
    public virtual void UpdateState()
    {
        // Reset state of the spoon object
        IsFilled = false;

        // Set default empty spoon sprite
        spoonSpriteRenderer.sprite = spoonSprites[0];
    }

    /// <summary>
    /// Fully resets spoon when the step is completed.
    /// </summary>
    protected virtual void Disable()
    {
        enabled = false;
        IsFilled = false;

        // Restore original position
        transform.position = _defaultPosition;

        // Restore default sprite
        spoonSpriteRenderer.sprite = spoonSprites[0];
    }
}
