using UnityEngine;

public class DumplingsSixthStepExecutable : BaseStepManager
{
    [SerializeField] private TimeIndicator timeIndicator;

    private bool _enableSouses;

    public override void Execute()
    {
        _enableSouses = false;

        base.Execute();
    }

    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        if (queryObject is SousManager)
            SetCompleted();
        else if (queryObject is DumplingsSpoonController)
            EnableSouses();
    }

    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        if (queryObject is SousManager)
            return _enableSouses;

        return false;
    }

    protected override void SetCompleted() 
    {
        _enableSouses = false;
        base.SetCompleted();
    }

    private void EnableSouses() { _enableSouses = true; }

    public override void EndExecution()
    {
        timeIndicator.HideIndicator();
        base.EndExecution();
    }
}
