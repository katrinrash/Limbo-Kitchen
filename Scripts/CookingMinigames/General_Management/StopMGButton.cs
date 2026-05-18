using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI button controller responsible for manually stopping the current minigame session.
/// </summary>
///
/// Flow:
/// Player clicks stop → state machine reset → minigame ends → UI cleared and minigame elements hidden

public class StopMGButton : InstanceBaseClass<StopMGButton>
{
    // Reference to the state machine controlling the minigame flow
    private BaseStateMachine stateMachine;

    // Cached UI Button component
    private Button _button;

    private void Start()
    {
        // Cache reference to the Button component for event binding
        _button = GetComponent<Button>();

        // Bind stop functionality to UI click event
        _button.onClick.AddListener(Stop);
    }

    /// <summary>
    /// Assigns the state machine instance, allowing it to reset the minigame flow when stopping.
    /// </summary>
    /// <param name="machine">Current minigame state machine.</param>
    public void SetStateMachine(BaseStateMachine machine)
    {
        stateMachine = machine;
    }

    /// <summary>
    /// Entry point triggered by UI button click.
    /// </summary>
    private void Stop()
    {
        StopMinigame();
    }

    /// <summary>
    /// Handles full cleanup of the minigame session. Resets state machine and disables UI logic.
    /// </summary>
    private void StopMinigame()
    {
        // Reset state machine 
        stateMachine.SetState();

        // Notify global manager that minigame ended unsuccessfully, so it can be played again.
        MinigamesManager.Instance.EndMinigame(false);

        // Hide UI if it is active
        SwitchBTNLogic.Instance.gameObject.SetActive(false);
    }
}
