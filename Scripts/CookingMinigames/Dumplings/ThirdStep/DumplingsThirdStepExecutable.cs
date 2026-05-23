using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executable for the third step of the dumpling preparation minigame. 
/// </summary>

public class DumplingsThirdStepExecutable : BaseStepManager
{
    // Required number of dumplings to complete this step
    [SerializeField] private int dumplingsCount;

    // Reference to spoon controller used for filling logic
    [SerializeField] private FillingSpoonController spoon;

    // Current number of filled dumplings
    private int _filledDumplingsCount;

    /// <summary>
    /// Initializes the third step of the dumpling minigame.
    /// </summary>
    public override void Execute()
    {
        // Reset filled dumplings counter at the start of the step
        _filledDumplingsCount = 0;

        // Continue standard step initialization 
        base.Execute();
    }

    /// <summary>
    /// Updates step progress after successful interaction.
    /// </summary>
    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        // Increment filled dumplings count after each successful spoon interaction
        _filledDumplingsCount++;

        // Update spoon visual state after each filled dumpling
        spoon.UpdateState();

        // Complete step when required amount is reached
        if (_filledDumplingsCount == dumplingsCount)
            SetCompleted();
    }

    /// <summary>
    /// Handles availability logic for interactions during this step. Only allows interaction when the spoon is filled and ready to be used.
    /// </summary>
    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return spoon.IsFilled;
    }
}
