using Unity.VisualScripting;
using UnityEngine.UI;

// Controls the switch button used to progress between minigame stages.
//
// Flow:
// Step completes requirements → Button appears → Player presses the button → State progresses

public class SwitchBTNLogic : InstanceBaseClass<SwitchBTNLogic>
{
    private IExecutable _step;

    protected override void OnInstanceBaseClassAwake()
    {
        gameObject.SetActive(false);
    }

    public void RequirementsCompleted(IExecutable step)
    {
        _step = step;
        gameObject.SetActive(true);
    }

    public void SetStageCompleted()
    {
        gameObject.SetActive(false);
        _step.SetAsCompleted();
    }
}
