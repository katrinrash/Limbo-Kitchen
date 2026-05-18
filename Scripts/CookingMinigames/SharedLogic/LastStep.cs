using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Final step of the minigame sequence responsible for displaying the completed dish result to the player.
/// </summary>
///
/// Responsibilities:
/// - Show final result sprite
/// - Play corresponding result audio
/// - Notify UI/game flow systems about completion
///
/// Architecture:
/// - Implements IExecutable for state machine integration
/// - Uses OrderValidator to determine result outcome
/// - Acts as the presentation stage of the minigame pipeline

public class LastStep : MonoBehaviour, IExecutable
{
    // Collection of possible result sprites
    [SerializeField] protected Sprite[] results;

    // UI image used to display final result
    [SerializeField] private Image result;

    /// <summary>
    /// Reference used to notify the state system when this step is completed.
    /// </summary>
    public IStateHandler StateHandler { get; set; }

    /// <summary>
    /// Activates this step and initializes its gameplay logic.
    /// </summary>
    public void Execute()
    {
        // Notifies the switch button system that this step has satisfied its completion requirements, so the player can move on.
        SwitchBTNLogic.Instance.RequirementsCompleted(this);

        // Select and set sprite based on calculated dish result
        result.sprite = results[OrderValidator.Instance.GetDishResultIndex()];

        // Display result image
        result.gameObject.SetActive(true);

        // Play audio feedback corresponding to the final result
        AudioManager.Instance.PlayResult(OrderValidator.Instance.GetDishResultIndex());

        // Enable step object itself
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Ends execution of this step and performs cleanup.
    /// </summary>
    public void EndExecution()
    {
        // Hide result image
        result.gameObject.SetActive(false);

        // Disable step object itself
        gameObject.SetActive(false);
    }
}
