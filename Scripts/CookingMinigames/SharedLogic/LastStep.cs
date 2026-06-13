using UnityEngine;
using UnityEngine.UI;

// Final step of the minigame sequence responsible for displaying the completed dish result to the player.
//
// Responsibilities:
// - Show final result sprite
// - Play corresponding result audio
// - Notify UI/game flow systems about completion
//
// Architecture:
// - Implements IExecutable for state machine integration
// - Uses OrderValidator to determine result outcome
// - Acts as the presentation stage of the minigame pipeline

public class LastStep : MonoBehaviour, IExecutable
{
    [SerializeField] protected Sprite[] results;
    [SerializeField] private Image result;

    public IStateHandler StateHandler { get; set; }

    public void Execute()
    {
        SwitchBTNLogic.Instance.RequirementsCompleted(this);

        result.sprite = results[OrderValidator.Instance.GetDishResultIndex()];
        result.gameObject.SetActive(true);

        AudioManager.Instance.PlayResult(OrderValidator.Instance.GetDishResultIndex());

        gameObject.SetActive(true);
    }

    public void EndExecution()
    {
        result.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
