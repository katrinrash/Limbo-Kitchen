using UnityEngine;

public class TeaThirdStepExecutable : BaseStepManager
{
    private bool TeaChosen = false;

    public override void Execute()
    {
        TeaChosen = false;
        base.Execute();
    }

    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        base.UpdateAfterUsed(queryObject);
        TeaChosen = true;
    }

    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return TeaChosen;
    }

}
