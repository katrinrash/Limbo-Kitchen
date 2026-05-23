using UnityEngine;

public class DumplingsFifthStepExecutable : BaseStepManager
{
    [SerializeField] private TimerSkillCheckLogic timerSkillCheckLogic;
    [SerializeField] private int _totalAmountToBoil;
    [SerializeField] private BoilingAnimationManager boilingAnimationManager;

    private int _boiledCount;

    public override void Execute()
    {
        _boiledCount = 0;

        base.Execute();
    }

    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        if (!queryObject) return;

        if (queryObject is BoilingComponentController)
            BoilComponent();
        else if (queryObject is TimerSkillCheckLogic)
        {
            if (timerSkillCheckLogic.SkillCheckValid)
                OnSucceeded();
            else
                CompleteSkillCheck();
        }
    }

    private void BoilComponent()
    {
        if (++_boiledCount >= _totalAmountToBoil)
        {
            timerSkillCheckLogic.Initialize();
            boilingAnimationManager.StartBoiling();
        }
    }

    private void OnSucceeded()
    {
        OrderValidator.Instance.SetSuccessfulSkillCheck();
    }

    private void CompleteSkillCheck() // figure out step completion with designer
    {
        StateHandler?.CompleteStep();
    }

}
