using UnityEngine;

public class MovableGaiwanController : GaiwanManager
{
    [SerializeField] private RemoveContainerController containerController;
    [SerializeField] private WaitingIndicatorLogic waitingIndicator;
    [SerializeField] private ActionAnimationManager actionAnimationManager;
    [SerializeField] protected SpriteRenderer animationObject;
    [SerializeField] protected Sprite[] animations;
    [SerializeField] protected float speed;

    private Vector3 _startPos;

    #region UnityMethods

    protected override void Awake()
    {
        base.Awake();
        stepManager.OnStepCompleted += ResetGaiwan;
        _startPos = transform.position;
        enabled = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        stepManager.OnStepCompleted -= ResetGaiwan;
    }

    #endregion

    #region Unity Lifecycle
    public override void OnRaycast()
    {
        enabled = true;
    }

    void Update()
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

    private void Check()
    {
        if (Raycaster.CheckRaycastResult(containerController, transform.position) && !IsClosed && containerController.IsReady)
        {
            enabled = false;
            IsEmpty = true;
            actionAnimationManager.PlayRemoveAnimation(this, _startPos, animations, animationObject, speed);
        }
    }

    #endregion

    public void StartSkillCheck()
    {
        waitingIndicator.StartProcess();
    }

    protected override void OnContentChanged() // to prevent base logic execution
    {
    }

    public override void OnCompleted()
    {
        containerController.AddWater();
        stepManager.UpdateAfterUsed();
    }

    private void ResetGaiwan()
    {
        transform.position = _startPos;
        animationObject.sprite = animations[0];
    }
}
