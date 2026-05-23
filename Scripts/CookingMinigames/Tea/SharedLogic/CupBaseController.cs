using UnityEngine;

public class CupBaseController : InteractableObject
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private MovableGaiwanController gaiwan;
    [SerializeField] private WaitingIndicatorLogic indicator;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform parent;

    private Vector3 _startPos;
    private bool _isUsed = false;
    private bool _enableToReuse;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += ResetLid;
        indicator.OnCompleted += EnableToReuse;
        _startPos = transform.position;
        enabled = false;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetLid;
        indicator.OnCompleted -= EnableToReuse;
    }

    #endregion

    public override void OnRaycast()
    {
        if (_isUsed) return;

        enabled = true;
    }

    private void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        if (transform.position != _startPos)
        {
            if(_enableToReuse)
            {
                transform.SetParent(parent);
                gaiwan.ChangeAvailability(); 
                _isUsed = true;
            }

            transform.position = _startPos;
            _enableToReuse = false;
            enabled = false;
        }
    }

    private void Check()
    {
        if (_enableToReuse) return;
        
        if (Raycaster.CheckRaycastResult(gaiwan, transform.position) && !gaiwan.IsEmpty)
        {
            _isUsed = true;
            enabled = false;
            gaiwan.ChangeAvailability();
            transform.SetParent(gaiwan.transform);
            AudioManager.Instance.PlayCloseLid();
            transform.position = endPos.position;
            gaiwan.StartSkillCheck();
        }
    }

    private void EnableToReuse()
    {
        _isUsed = false;
        _enableToReuse = true;
    }

    private void ResetLid()
    {
        transform.position = _startPos;
        enabled = false;
        _isUsed = false;
        _enableToReuse = false;
    }
}
