using System;
using UnityEngine;

// Executable for the second step of the dumpling preparation minigame. 
//
// Responsibilities:
// - Track added ingredients
// - Update bowl visuals based on preparation progress
// - Validate when mixing can be completed
// - Control transition to the next gameplay stage

public class DumplingsSecondStepExecutable : BaseStepManager
{
    [SerializeField] private SpriteRenderer bowl;
    [SerializeField] private Sprite[] bowls;
    [SerializeField] private int maxIngredientAmount = 3;

    public bool IngredientsUsedUp => _usedIngredients == maxIngredientAmount;

    private int _usedIngredients = 0;

    public override void Execute()
    {
        _usedIngredients = 0;
        bowl.sprite = bowls[0];

        base.Execute();
    }

    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return IngredientsUsedUp;
    }

    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        if (!queryObject) return;

        if (queryObject is IngredientManager)
            IngredientUsed();

        else if (queryObject is SpoonController)
            SetCompleted();
    }

    private void IngredientUsed()
    {
        _usedIngredients++;

        if (IngredientsUsedUp)
            bowl.sprite = bowls[1];
    }
}
