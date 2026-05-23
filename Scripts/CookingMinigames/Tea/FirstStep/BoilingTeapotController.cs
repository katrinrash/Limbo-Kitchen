using System.Collections;
using UnityEngine;

public class BoilingTeapotController : TeapotBase
{
    [SerializeField] private Transform endPosition;
    [SerializeField] private BoilingIndicatorLogic boilingIndicator;
    [SerializeField] private Switch_Controller switchController; 
    [SerializeField] private string targetTag;

    private Vector3 _endPosition;
    private bool _isBoiled = false;

    #region UnityMethods

    protected override void Awake()
    {
        base.Awake();
        boilingIndicator.OnCompleted += CompleteBoiling;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        boilingIndicator.OnCompleted -= CompleteBoiling;
    }

    #endregion

    #region Unity Preparation

    public override void Initialize()
    {
        base.Initialize();
        _endPosition = endPosition.localPosition;
        _isBoiled = false;
    }

    public override void ResetTeapot()
    {
        base.ResetTeapot();
        ResetBoiling();
    }

    private void ResetBoiling()
    {
        animationObject.sprite = null;
        StopAllCoroutines();
        AudioManager.Instance.StopBoilingTeapot();
    }

    #endregion

    #region Unity Lifecycle

    public override void OnRaycast() 
    {
        if (_isBoiled) return;

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

        transform.position = _startPosition;

        if(_isUsed)
        {
            switchController.ChangeContent(); 
            _isUsed = false;
        }

        enabled = false;

    }

    private void Check()
    {
        if (!_isUsed && Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            _isUsed = true;
            switchController.ChangeContent();
            transform.position = _endPosition;
            enabled = false;
            return;
        }

        else if (_isUsed) CheckCurrentState();

    }

    private void CheckCurrentState()
    {
        if (boilingIndicator.BoilingStarted())
        {
            if (!_isBoiled)
                boilingIndicator.ValidateTeapot();
            _isBoiled = true;
            ResetBoiling();
            stepManager.UpdateAfterUsed();
        }
    }

    #endregion

    #region Boiling 

    private IEnumerator Boiled()
    {
        int i = 0;
        animationObject.sprite = animations[i++];

        while(true)
        {
            yield return new WaitForSeconds(speed);

            animationObject.sprite = animations[i++];

            if (i == animations.Length) i = 0;

        }
    }

    private void CompleteBoiling()
    {
        StartCoroutine(Boiled());
        AudioManager.Instance.PlayBoilingTeapot();
    }

    #endregion

}
