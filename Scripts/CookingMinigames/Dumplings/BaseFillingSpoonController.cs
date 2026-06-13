using UnityEngine;

// Base controller for spoon interactable objects. Handles shared logic to reduce code duplication across different spoon types.
public class BaseFillingSpoonController : InteractableObject
{
    [SerializeField] protected SpriteRenderer spoonSpriteRenderer;
    [SerializeField] protected BaseStepManager stepManager;
    [SerializeField] protected Sprite[] spoonSprites;
    [SerializeField] protected string targetTag;

    public bool IsFilled { get; protected set; } = false;

    protected Vector3 _defaultPosition;

    #region UnityMethods

    private void Awake()
    {
        _defaultPosition = transform.position;
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
        enabled = true;
    }

    protected virtual void Check() { }

    public virtual void UpdateState()
    {
        IsFilled = false;
        spoonSpriteRenderer.sprite = spoonSprites[0];
    }

    protected virtual void Disable()
    {
        enabled = false;
        IsFilled = false;

        transform.position = _defaultPosition;
        spoonSpriteRenderer.sprite = spoonSprites[0];
    }
}
