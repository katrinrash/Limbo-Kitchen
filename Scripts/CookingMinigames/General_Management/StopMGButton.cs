using UnityEngine;
using UnityEngine.UI;

public class StopMGButton : InstanceBaseClass<StopMGButton>
{
    private BaseStateMachine stateMachine;
    private Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Stop);
    }

    public void SetStateMachine(BaseStateMachine machine)
    {
        stateMachine = machine;
    }

    public void Stop()
    {
        StopMinigame();
        //FaderManager.Instance.FadeIn();
        //Invoke(nameof(StopMinigame), FaderManager.Instance.FadeDuration);
    }

    private void StopMinigame()
    {
        stateMachine.SetState();
        MinigamesManager.Instance.EndMinigame(false);
        SwitchBTNLogic.Instance.gameObject.SetActive(false);
    }

}
