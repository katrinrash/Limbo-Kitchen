using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorBaseLogic : MonoBehaviour //update with coroutines usage and indicators usage later 
{
    public Action OnCompleted;

    [SerializeField] protected BaseStepManager stepManager;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected GameObject indicator;
    [SerializeField] protected float speed;
    [SerializeField] protected float speedRush;

    protected float _fillAmount;
    protected float _currentSpeed;

    #region UnityMethods

    protected virtual void Awake()
    {
        if (!stepManager) return; //tempo
        
        stepManager.OnStepStarted += Initialize;
        stepManager.OnStepCompleted += HideIndicator;
        stepManager.OnStepCompleted += StopProcess;
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        if (!stepManager) return; //tempo

        stepManager.OnStepStarted -= Initialize;
        stepManager.OnStepCompleted -= HideIndicator;
        stepManager.OnStepCompleted -= StopProcess;
    }

    #endregion

    public virtual void Initialize() // switch to private later!
    {
        _currentSpeed = ShiftManager.Instance.IsPeakHour? speedRush : speed;
        enabled = true;
    }

    public virtual void StartProcess()
    {
        StartCoroutine(IncreaseIndicator());
    }

    public virtual void StopProcess() 
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator IncreaseIndicator()
    {
        while (_fillAmount < 1)
        {
            _fillAmount += speed * Time.deltaTime;
            fillImage.fillAmount = _fillAmount;
            yield return null;
        }

        OnProcessCompleted();
    }

    protected IEnumerator DecreaseIndicator()
    {
        while (_fillAmount > 0)
        {
            _fillAmount -= speed * Time.deltaTime;
            fillImage.fillAmount = _fillAmount;
            yield return null;
        }
    }


    protected virtual void OnProcessCompleted()
    {
    }

    public virtual void HideIndicator() // switch to private later!
    {
        indicator.SetActive(false);
        fillImage.gameObject.SetActive(false);

        ResetVisual();
    }

    private void ResetVisual() 
    {
        _fillAmount = 0;
        fillImage.fillAmount = _fillAmount;
    }

    public virtual void ShowIndicator() // switch to private later!
    {
        indicator.SetActive(true);
        fillImage.gameObject.SetActive(true);
    }
}
