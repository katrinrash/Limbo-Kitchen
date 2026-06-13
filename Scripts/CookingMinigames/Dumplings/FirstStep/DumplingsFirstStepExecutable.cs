using System;
using UnityEngine;

// Executable for the first step of the dumpling preparation minigame. 
public class DumplingsFirstStepExecutable : BaseStepManager
{
    [SerializeField] private float stepPrice = 0.5f;

    public override void Execute()
    {
        CurrencyManager.Instance.UseCurrency(stepPrice);

        base.Execute();
    }
}
