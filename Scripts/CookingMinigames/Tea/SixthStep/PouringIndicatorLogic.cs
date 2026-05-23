using UnityEngine;

public class PouringIndicatorLogic : IndicatorBaseLogic 
{
    [Header("Poring Indicators Specific Data")]

    [SerializeField] private ReducableObjectController reducableObjectController;
    [SerializeField] private float min = 0.55f;
    [SerializeField] private float max = 0.65f;
    [SerializeField] private bool modifiedBehavior = false;

    #region UnityMethods

    protected override void Awake()
    {
        base.Awake();
        reducableObjectController.OnStartPouring += StartProcess;
        reducableObjectController.OnStopPouring += StopProcess;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        reducableObjectController.OnStartPouring -= StartProcess;
        reducableObjectController.OnStopPouring -= StopProcess;
    }

    #endregion

    public override void Initialize()
    {
        ShowIndicator();
        base.Initialize();
    }

    public override void HideIndicator()
    {
        if (ValidReaction() && !modifiedBehavior)
            OrderValidator.Instance.SetSuccessfulSkillCheck();

        base.HideIndicator();
    }

    private bool ValidReaction()
    {
        return _fillAmount >= min && _fillAmount <= max;
    }

    protected override void OnProcessCompleted() 
    {
        reducableObjectController.StopReduction();
        reducableObjectController.RestrictUsage();

        if (modifiedBehavior)
            stepManager.UpdateAfterUsed();
    }
}
