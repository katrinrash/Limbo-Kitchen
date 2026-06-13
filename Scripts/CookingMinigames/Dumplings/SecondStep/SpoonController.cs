using UnityEngine;
using UnityEngine.InputSystem;

// Controls spoon interaction for mixing mechanic in the minigame.
//
// Responsibilities:
// - Allow player to pick up and move spoon
// - Detect interaction with target bowl area
// - Track mixing progress over time
// - Lock interaction when mixing is completed
// - Reset state after step completion
//
// Interaction Flow:
// Pick spoon → drag → enter bowl → mix over time → complete step

public class SpoonController : InteractableObject
{
    [Header("Base Data")]

    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private string targetTag;

    [Header("Movement Constraints Data")]

    [SerializeField] private Transform newRotation;
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;
    [SerializeField] private float timeToCook = 5f;
    [SerializeField] private float newZ;

    public bool IsAvailable { get; private set; } = true;

    private float _elapsedTime = 0;
    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;
    private bool _inBowl = false;

    #region UnityMethods

    private void Awake()
    {
        _defaultPosition = transform.position;
        _defaultRotation = transform.localRotation;

        stepManager.OnStepCompleted += Disable;
        enabled = false;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    public override void OnRaycast()
    {
        if (!IsAvailable || !stepManager.GetAvailabilityInfo())
            return;

        enabled = true;
        transform.rotation = newRotation.rotation;
    }

    private void Update()
    {
        if (!IsAvailable)
        {
            AudioManager.Instance.StopMixSound();

            transform.position = _defaultPosition;
            transform.localRotation = _defaultRotation;

            enabled = false;
            return;
        }

        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            return;
        }

        AudioManager.Instance.StopMixSound();
        transform.position = _defaultPosition;
        transform.localRotation = _defaultRotation;

        _inBowl = false;
        enabled = false;
    }

    protected override void Move()
    {
        if (!_inBowl)
        {
            base.Move();
            Check();
            return;
        }

        Vector2 delta = Mouse.current.delta.ReadValue() * 0.01f;

        Vector3 pos = transform.localPosition;

        pos.x += delta.x;
        pos.y += delta.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.z = newZ;

        transform.localPosition = pos;

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= timeToCook)
        {
            stepManager.UpdateAfterUsed(this);
            IsAvailable = false;
        }
    }

    private void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            AudioManager.Instance.PlayMixSound();
            _inBowl = true;
        }
    }

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
