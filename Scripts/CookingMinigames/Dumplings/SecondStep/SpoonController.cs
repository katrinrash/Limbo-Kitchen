using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls spoon interaction for mixing mechanic in the minigame.
/// </summary>
///
/// Responsibilities:
/// - Allow player to pick up and move spoon
/// - Detect interaction with target bowl area
/// - Track mixing progress over time
/// - Lock interaction when mixing is completed
/// - Reset state after step completion
///
/// Interaction Flow:
/// Pick spoon → drag → enter bowl → mix over time → complete step

public class SpoonController : InteractableObject
{
    [Header("Base Data")]
    // Step manager controlling current gameplay stage
    [SerializeField] private BaseStepManager stepManager;

    // Tag used to detect valid bowl interaction zone
    [SerializeField] private string targetTag;

    [Header("Movement Constraints Data")]

    // Rotation applied when spoon is picked up
    [SerializeField] private Transform newRotation;

    // Movement boundaries for constrained mixing 
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    // Time required to complete mixing process
    [SerializeField] private float timeToCook = 5f;

    // Fixed Z-position used during interaction
    [SerializeField] private float newZ;

#endregion

    /// <summary>
    /// Boolean indicating whether the spoon can currently be interacted with. Becomes false after mixing is completed to prevent further interaction until reset.
    /// </summary>
    public bool IsAvailable { get; private set; } = true;

    // Tracks elapsed mixing time
    private float _elapsedTime = 0;

    // Default transform state for reset
    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;

    // Whether spoon is currently inside bowl interaction zone
    private bool _inBowl = false;

    #region UnityMethods

    private void Awake()
    {
        // Cache default transform state for reset logic
        _defaultPosition = transform.position;
        _defaultRotation = transform.localRotation;

        // Subscribe to step completion event to reset when the step is completed
        stepManager.OnStepCompleted += Disable;

        // Disabled until interaction begins
        enabled = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe from step completion event
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    /// <summary>
    /// Called when spoon is hit by raycast.
    /// </summary>
    public override void OnRaycast()
    {
        // If spoon is not available or step conditions are not met, do not allow interaction
        if (!IsAvailable || !stepManager.GetAvailabilityInfo())
            return;

        enabled = true;

        // Rotate spoon into "active" interaction pose
        transform.rotation = newRotation.rotation;
    }

    private void Update()
    {
        // If spoon is no longer usable, force reset state
        if (!IsAvailable)
        {
            // Manage audio to ensure mixing sound is stopped if spoon becomes unavailable
            AudioManager.Instance.StopMixSound();

            // Reset position and rotation to default state
            transform.position = _defaultPosition;
            transform.localRotation = _defaultRotation;

            // Disable interaction 
            enabled = false;
            return;
        }

        // Manage movement while mouse input is held
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            return;
        }

        // If input released, reset state and stop mixing sound if it was active
        AudioManager.Instance.StopMixSound();
        transform.position = _defaultPosition;
        transform.localRotation = _defaultRotation;

        _inBowl = false;
        enabled = false;
    }

    /// <summary>
    /// Custom movement logic:
    /// Outside bowl: normal drag + detection,
    /// Inside bowl: constrained mixing movement + progress tracking
    /// </summary>
    protected override void Move()
    {
        // Before entering bowl, allow free movement and check for bowl interaction
        if (!_inBowl)
        {
            base.Move();
            Check();
            return;
        }

        // Mouse delta-based controlled mixing movement
        Vector2 delta = Mouse.current.delta.ReadValue() * 0.01f;

        Vector3 pos = transform.localPosition;

        pos.x += delta.x;
        pos.y += delta.y;

        // Clamp movement inside allowed bowl area
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.z = newZ;

        transform.localPosition = pos;

        // Track mixing progress
        _elapsedTime += Time.deltaTime;

        // Complete step when required time is reached
        if (_elapsedTime >= timeToCook)
        {
            stepManager.UpdateAfterUsed(this);
            IsAvailable = false;
        }
    }

    /// <summary>
    /// Checks if spoon is positioned over valid bowl area.
    /// </summary>
    private void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            // Start mixing sound when spoon enters bowl area
            AudioManager.Instance.PlayMixSound();
            _inBowl = true;
        }
    }

    /// <summary>
    /// Resets spoon object after step completion.
    /// </summary>
    private void Disable()
    {
        enabled = false;

        _inBowl = false;
        IsAvailable = true;

        _elapsedTime = 0f;

        transform.position = _defaultPosition;
        transform.localRotation = _defaultRotation;
    }
}
