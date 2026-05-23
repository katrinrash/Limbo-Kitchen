using UnityEngine;

public class TeaSeventhStepExecutable : BaseStepManager
{
    [SerializeField] private TimeIndicator timeIndicator;

    public override void EndExecution()
    {
        timeIndicator.HideIndicator();
        base.EndExecution();
    }
}
