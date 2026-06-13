using System.Collections.Generic;
using UnityEngine;

// Executable for the third step of the dumpling preparation minigame. 
public class DumplingsThirdStepExecutable : BaseStepManager
{
    [SerializeField] private int dumplingsCount;
    [SerializeField] private FillingSpoonController spoon;

    private int _filledDumplingsCount;

    public override void Execute()
    {
        _filledDumplingsCount = 0;

        base.Execute();
    }

    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        _filledDumplingsCount++;
        spoon.UpdateState();

        if (_filledDumplingsCount == dumplingsCount)
            SetCompleted();
    }

    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return spoon.IsFilled;
    }
}
