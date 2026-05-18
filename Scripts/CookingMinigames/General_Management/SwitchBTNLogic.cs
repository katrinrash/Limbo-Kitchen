using Unity.VisualScripting;
using UnityEngine.UI;

/// <summary>
/// Controls the switch button used to progress between minigame stages.
/// </summary>
///
/// Flow:
/// Step completes requirements → Button appears → Player presses the button → State progresses

public class SwitchBTNLogic : InstanceBaseClass<SwitchBTNLogic>
{
    // Reference to the currently active executable step
    private IExecutable _step;

    /// <summary>
    /// Hides the button on initialization.
    /// </summary>
    protected override void OnInstanceBaseClassAwake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when a gameplay step satisfies its completion conditions. Enables the confirmation button for player interaction.
    /// </summary>
    /// 
    /// <param name="step">
    /// Current executable step awaiting confirmation.
    /// </param>
    public void RequirementsCompleted(IExecutable step)
    {
        // Cache reference to the active step 
        _step = step;

        // Show the button
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Confirms current stage completion and notifies the state system to proceed.
    /// </summary>
    public void SetStageCompleted()
    {
        // Hide the button
        gameObject.SetActive(false);

        // Notify current step that execution is complete, allowing it to perform any necessary cleanup and trigger state progression
        _step.SetAsCompleted();
    }
}
