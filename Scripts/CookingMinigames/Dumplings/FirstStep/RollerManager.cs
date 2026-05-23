using System.Collections;
using UnityEngine;

/// <summary>
/// Controls dough rolling interaction for the dumpling minigame. Handles rolling animation, dough state progression, and completion logic.
/// </summary>
///
/// Flow:
/// Player interaction → Roller animation → Dough state update → Repeat until step completion

public class RollerManager : InteractableObject
{
    [Header("Base Data")]

    // Step manager controlling current gameplay stage
    [SerializeField] private BaseStepManager stepManager;

    // Reference to the sprite renderer used to update visuals
    [SerializeField] private SpriteRenderer doughSpriteRenderer;

    // An array of sprites representing different dough states
    [SerializeField] private Sprite[] doughStates;

    [Header("Animation Details")]

    // Upper animation target position
    [SerializeField] private Vector3 upPosition;

    // Lower - default animation target position
    [SerializeField] private Vector3 downPosition;

    // Speed multiplier for rolling animation
    [SerializeField] private float _rollingSpeed = 1f;

    // Current rolling progress index
    private int CurrentIteration = 0;

    // Maximum amount of rolling iterations required
    private int _maxAmountOfIterations = 0;

    // Prevents overlapping animations/interactions
    private bool _isWorking = false;

    #region UnityMethods

    private void Awake()
    {
        // Subscribe to step completion event to reset when the step is completed
        stepManager.OnStepCompleted += ResetComponent;

        // Set necessary amount of iterations based on available dough states
        _maxAmountOfIterations = doughStates.Length - 1;
    }

    private void OnDestroy()
    {
        // Unsubscribe from step completion event
        stepManager.OnStepCompleted -= ResetComponent;
    }

    #endregion

    /// <summary>
    /// Handles interaction when roller object is hit by a raycast.
    /// </summary>
    public override void OnRaycast()
    {
        // Prevent interaction while animation is active or when rolling is already completed
        if (_isWorking || CurrentIteration == _maxAmountOfIterations)
            return;

        // Start rolling animation
        StartCoroutine(MoveRollerCoroutine());

        // Manage audio feedback for rolling action 
        AudioManager.Instance.PlayDoughSound();
    }

    /// <summary>
    /// Smoothly animates the roller between two positions.
    /// </summary>
    private IEnumerator MoveRollerCoroutine()
    {
        // Lock interaction while animation is active
        _isWorking = true;

        // Cache starting position for interpolation
        var startPosition = transform.localPosition;

        // Alternate movement direction each interaction
        var targetPosition = transform.localPosition == upPosition ? downPosition : upPosition;

        // Index used for interpolation progress
        float positionIndex = 0f;

        // Animate roller movement until target position is reached
        while (transform.localPosition != targetPosition)
        {
            // Increment position index based on time and speed
            positionIndex += Time.deltaTime * _rollingSpeed;

            // Smoothly interpolate roller position between start and target
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, positionIndex);

            yield return null;
        }

        // Call method to update dough state after each successful roll
        UpdateDoughState();

        // Unlock interaction after animation completes
        _isWorking = false;
    }

    /// <summary>
    /// Updates the dough sprite to reflect current rolling progress and checks for step completion.
    /// </summary>
    private void UpdateDoughState()
    {
        // Move to next dough sprite
        doughSpriteRenderer.sprite = doughStates[++CurrentIteration];

        // Check if maximum rolling iterations have been reached to trigger step completion
        if (CurrentIteration == _maxAmountOfIterations)
        {
            stepManager.UpdateAfterUsed();
            return;
        }
    }

    /// <summary>
    /// Resets components after step completion.
    /// </summary>
    private void ResetComponent()
    {
        // Stop all ongoing animations
        StopAllCoroutines();

        // Reset rolling progress index
        CurrentIteration = 0;

        // Restore initial dough appearance
        doughSpriteRenderer.sprite = doughStates[CurrentIteration];

        // Reset roller default position
        transform.localPosition = downPosition;

        // Unlock future interactions
        _isWorking = false;
    }
}
