using UnityEngine;

public class ContainerBaseController : InteractableObject
{
    [SerializeField] protected BaseStepManager stepManager;
    [SerializeField] protected ActionAnimationManager actionAnimationManager;
    [SerializeField] protected string targetTag;
    [SerializeField] protected SpriteRenderer animationObject;
    [SerializeField] protected Sprite[] animations;
    [SerializeField] protected float speed;

    protected Vector3 _startPos;

    #region UnityMethods

    protected virtual void Awake()
    {
        stepManager.OnStepCompleted += ResetContainer;
        _startPos = transform.position;
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetContainer;
    }

    #endregion

    protected void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        if (transform.position != _startPos)
        {
            transform.position = _startPos;
            enabled = false;
        }
    }

    protected virtual void Check() { }

    protected virtual void ResetContainer()
    { 
        transform.position = _startPos;
        animationObject.sprite = animations[0];
        enabled = false;
    }
}
