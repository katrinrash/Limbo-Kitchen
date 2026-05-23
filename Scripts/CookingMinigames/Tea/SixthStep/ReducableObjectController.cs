using System;
using System.Collections;
using UnityEngine;

public class ReducableObjectController : InteractableObject // needs improvement
{
    public Action OnStartPouring;
    public Action OnStopPouring;

    [SerializeField] protected BaseStepManager stepManager;
    [SerializeField] protected SpriteRenderer animationObject;
    [SerializeField] protected Sprite[] animations;
    [SerializeField] protected float speed;

    protected bool _Available = true;
    protected Coroutine _currentCoroutine;
    protected int _index;

    #region UnityMethods

    protected virtual void Awake()
    {
        stepManager.OnStepCompleted += ResetObject;
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetObject;
    }

    #endregion

    #region Unity Lifecycle
    public override void OnRaycast()
    {
        if(!_Available) return;

        StartCurrentCoroutine(Pour());
        enabled = true;
    }

    protected virtual void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed() && Check()) return;

        StopReduction();
    }

    private bool Check()
    {
        if (Raycaster.CheckRaycastResult(this, RaycastUtility.GetPoint())) return true;

        return false;
    }

    public void RestrictUsage()
    {
        _Available = false;
    }

    #endregion

    public virtual void StopReduction()
    {
        OnStopPouring?.Invoke();
        StartCurrentCoroutine(StopPouring());
        enabled = false;
    }

    protected virtual void ResetObject()
    {
        ResetCoroutine();
        _Available = true;
        animationObject.sprite = animations[0];
        _index = 0;
        enabled = false;
    }

    #region Coroutines
    protected virtual void StartCurrentCoroutine(IEnumerator routine)
    {
        ResetCoroutine();

        _currentCoroutine = StartCoroutine(routine);
    }

    protected virtual IEnumerator Pour() //?
    {
        OnStartPouring?.Invoke();

        if (_index >= animations.Length) _index = animations.Length - 1;
        else if(_index < 0) _index = 0;

        while (true)
        {
            animationObject.sprite = animations[_index];
            yield return new WaitForSeconds(speed);
            if (++_index == animations.Length) _index = animations.Length - 2;
        }
    }

    protected virtual IEnumerator StopPouring() //?
    {
        if (_index >= animations.Length) _index = animations.Length - 1;
        animationObject.sprite = animations[_index];

        while (_index - 1 >= 0)
        {
            yield return new WaitForSeconds(speed);
            --_index;

            animationObject.sprite = animations[_index];
        }
    }

    protected void ResetCoroutine()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
    #endregion
}
