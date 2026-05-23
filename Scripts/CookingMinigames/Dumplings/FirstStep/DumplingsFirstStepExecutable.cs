using System;
using UnityEngine;

/// <summary>
/// Executable for the first step of the dumpling preparation minigame. 
/// </summary>

public class DumplingsFirstStepExecutable : BaseStepManager
{
    /// <summary>
    /// Currency cost for this particular step. Charged at the start of the step execution.
    /// </summary>
    [SerializeField] private float stepPrice = 0.5f;

    /// <summary>
    /// Initializes the first step of the dumpling minigame.
    /// </summary>
    public override void Execute()
    {
        // Charge player before step begins
        CurrencyManager.Instance.UseCurrency(stepPrice);

        // Continue standard step initialization
        base.Execute();
    }
}
