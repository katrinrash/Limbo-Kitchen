using UnityEngine;

public class TeapotBase : InteractableObject
{
    [SerializeField] protected BaseStepManager stepManager;
    [SerializeField] protected SpriteRenderer animationObject;
    [SerializeField] protected Sprite[] animations;
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected float speed;

    protected Vector3 _startPosition;
    protected bool _isUsed = false;

    #region UnityMethods

    protected virtual void Awake()
    {
        stepManager.OnStepStarted += Initialize;
        stepManager.OnStepCompleted += ResetTeapot;
        _startPosition = transform.position;
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        stepManager.OnStepStarted -= Initialize;
        stepManager.OnStepCompleted -= ResetTeapot;
    }

    #endregion

    public virtual void Initialize()
    {
        _isUsed = false;
        enabled = false;
    }

    public virtual void ResetTeapot()
    {
        animationObject.sprite = defaultSprite;
        transform.position = _startPosition;
    }

}
