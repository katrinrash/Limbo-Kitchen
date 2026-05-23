using System.Collections;
using UnityEngine;

/// <summary>
/// Base interactable ingredient used in the minigames. 
/// </summary>
///
/// Responsibilities:
/// - Allow player to pick and drag ingredient
/// - Validate correct placement using raycast checks
/// - Trigger ingredient usage logic
/// - Animate ingredient usage
/// - Reset state after step completion

public class IngredientManager : InteractableObject
{
    [Header("Base Data")]

    // Step manager controlling current gameplay stage
    [SerializeField] protected BaseStepManager stepManager;

    [Header("Animation Data")]

    // Curve used to control movement animation easing
    [SerializeField] protected AnimationCurve animationCurve;

    // Duration of ingredient usage animation
    [SerializeField] protected float animationDuration = 1f;

    [Header("Ingredient Data")]

    // Type of ingredient used for validation logic
    [SerializeField] protected PreferencesEnum ingredientType;

    // Index used for special guest validation logic
    [SerializeField] protected int index;

    // Cost of using this ingredient
    [SerializeField] protected float ingredientPrice = 0.3f;

    // Tag used to validate correct interaction target
    [SerializeField] protected string targetTag;

    // Whether ingredient can currently be used
    protected bool _isAvailable = true;

    // Default spawn position for reset logic
    protected Vector3 _defaultPosition;

    #region UnityMethods

    protected virtual void Awake()
    {
        // Cache default position 
        _defaultPosition = transform.position;

        // Subscribe to step completion event to reset when the step is completed
        stepManager.OnStepCompleted += ResetIngredient;

        // Disabled until interaction starts
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        // Unsubscribe from step completion event
        stepManager.OnStepCompleted -= ResetIngredient;
    }

    #endregion

    private void Update()
    {
        // Move ingredient with mouse while input is held and interaction is available and call check function to validate interaction
        if (InputManager.Instance.MouseAction.IsPressed() && _isAvailable)
        {
            Move();
            Check();
            return;
        }

        // Reset position if released outside interaction
        else if (transform.position != _defaultPosition)
        {
            HandlePositionReset();
        }
    }

    /// <summary>
    /// Called when ingredient is hit by raycast.
    /// </summary>
    public override void OnRaycast()
    {
        // If ingredient is not available or step manager indicates interactions are blocked, do nothing
        if (!_isAvailable || stepManager.GetAvailabilityInfo())
            return;

        // Call method to handle ingredient being chosen for interaction
        HandleIngredientChosen();
    }

    /// <summary>
    /// Activates ingredient interaction state.
    /// </summary>
    protected virtual void HandleIngredientChosen()
    {
        enabled = true;
    }

    /// <summary>
    /// Checks whether ingredient is placed on valid target.
    /// </summary>
    protected virtual void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            // If raycast hits valid target, handle successful placement
            HandleRaycast();
        }
    }

    /// <summary>
    /// Handles successful placement of ingredient.
    /// </summary>
    protected virtual void HandleRaycast()
    {
        // Mark ingredient as used and disable further interaction
        _isAvailable = false;
        enabled = false;

        // Manage audio feedback 
        AudioManager.Instance.PlayAddIngredientSound();

        // Start ingredient usage animation
        StartCoroutine(UseIngredient());
    }

    /// <summary>
    /// Handles ingredient usage logic, including validation, step updates, currency deduction, and animation.
    /// </summary>
    protected virtual IEnumerator UseIngredient()
    {
        // Validate ingredient choice with order validator manager
        OrderValidator.Instance.ValidateIngredient(ingredientType, index);

        // Update step manager logic after using ingredient
        stepManager.UpdateAfterUsed(this);

        // Deduct currency for using the ingredient
        CurrencyManager.Instance.UseCurrency(ingredientPrice);

        // Animate ingredient moving back to origin when being used 
        var startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / animationDuration;

            // Smoothly interpolate ingredient position back to default using animation curve for easing
            transform.position = Vector3.Lerp(startPosition, _defaultPosition, animationCurve.Evaluate(t));

            yield return null;
        }
    }

    /// <summary>
    /// Resets ingredient position.
    /// </summary>
    protected virtual void HandlePositionReset()
    {
        transform.position = _defaultPosition;
        enabled = false;
    }

    /// <summary>
    /// Fully resets ingredient after step completion.
    /// </summary>
    protected virtual void ResetIngredient()
    {
        StopAllCoroutines();

        transform.SetPositionAndRotation(_defaultPosition, Quaternion.identity);

        _isAvailable = true;
        enabled = false;
    }
}
