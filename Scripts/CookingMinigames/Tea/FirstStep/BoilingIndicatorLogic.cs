using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingIndicatorLogic : IndicatorBaseLogic
{ 
    [SerializeField] private List<float> checkPositions;

    private int _index;
    private bool _reachedBoilingPoint;

    public override void Initialize()
    {
        _index = 0;
        _reachedBoilingPoint = false;
        ShowIndicator();
        base.Initialize();
    }

    protected override IEnumerator IncreaseIndicator()
    { 
        while (_fillAmount < 1)
        {
            if (!_reachedBoilingPoint && _fillAmount >= checkPositions[0])
            {
                CompleteBoiling();
                _reachedBoilingPoint = true;
            }

            _fillAmount += speed * Time.deltaTime;
            fillImage.fillAmount = _fillAmount;
            yield return null;
        }
    }

    #region Validation

    public void ValidateTeapot()
    {
        if (_fillAmount <= checkPositions[_index] && _index > 0)
            OrderValidator.Instance.SetSuccessfulSkillCheck();

        StopProcess();
    }

    public void ValidateSwitching()
    {
        if (_fillAmount >= checkPositions[_index] - 0.1 && _fillAmount <= checkPositions[_index] + 0.1)
            OrderValidator.Instance.SetSuccessfulSkillCheck();
        else if(_fillAmount <= checkPositions[_index] - 0.1)
            StopProcess();

        _index++;
    }

    #endregion

    #region Boiling

    public bool BoilingStarted()
    {
        return _fillAmount > 0;
    }

    private void CompleteBoiling()
    {
        OnCompleted?.Invoke();
    }

    #endregion
}
