using System.Collections.Generic;
using UnityEngine;

public class WaitingIndicatorLogic : IndicatorBaseLogic 
{
    [SerializeField] private List<StructForDictionary<GameObject, float>> fixPoints;

    private int _index;

    #region Step Preparation

    public override void Initialize()
    {
        _index = 0;
        base.Initialize();
    }

    public override void HideIndicator()
    {
        base.HideIndicator();
        fixPoints[_index].key.SetActive(false);
    }

    #endregion

    private void Update()
    {
        if (InputManager.Instance.ValidationAction.WasPressedThisFrame())
        {
            OnProcessCompleted();

            enabled = false;
        }
    }

    #region Indicator Processes

    public override void StartProcess()
    {
        ShowIndicator();
        SetRandomPoint();
        base.StartProcess();
    }

    public override void StopProcess()
    {
        base.StopProcess();

        if (ValidReaction())
            OrderValidator.Instance.SetSuccessfulSkillCheck();

        HideIndicator(); 
    }

    protected override void OnProcessCompleted()
    {
        OnCompleted?.Invoke();
        StopProcess();
    }

    #endregion

    private void SetRandomPoint()
    { 
        _index = Random.Range(0, fixPoints.Count);
        fixPoints[_index].key.SetActive(true);
    }

    public bool ValidReaction()
    { 
        return _fillAmount >= fixPoints[_index].value - 0.1 && _fillAmount <= fixPoints[_index].value + 0.1;
    }

}
