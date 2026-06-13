using UnityEngine;
using UnityEngine.UI;

// UI button controller responsible for manually stopping the current minigame session.
//
// Flow:
// Player clicks stop → state machine reset → minigame ends → UI cleared and minigame elements hidden

public class StopMGButton : InstanceBaseClass<StopMGButton>
{
    private BaseStateMachine stateMachine;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Stop);
    }

    public void SetStateMachine(BaseStateMachine machine)
    {
        stateMachine = machine;
    }

    private void Stop()
    {
        StopMinigame();
    }

    private void StopMinigame()
    {
        stateMachine.SetState();
        MinigamesManager.Instance.EndMinigame(false);
        SwitchBTNLogic.Instance.gameObject.SetActive(false);
    }
}
