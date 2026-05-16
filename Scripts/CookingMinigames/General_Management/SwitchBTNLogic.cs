using Unity.VisualScripting;
using UnityEngine.UI;

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
