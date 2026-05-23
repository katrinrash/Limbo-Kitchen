using System;
using UnityEngine;

/// <summary>
/// Executable for the second step of the dumpling preparation minigame. 
/// </summary>
///
/// Responsibilities:
/// - Track added ingredients
/// - Update bowl visuals based on preparation progress
/// - Validate when mixing can be completed
/// - Control transition to the next gameplay stage

public class DumplingsSecondStepExecutable : BaseStepManager
{
    // Reference to the sprite renderer used to update bowl visuals
    [SerializeField] private SpriteRenderer bowl;

    // An array of sprites representing different bowl states
    [SerializeField] private Sprite[] bowls;

    // Amount of ingredients required
    [SerializeField] private int maxIngredientAmount = 3;

    /// <summary>
    /// Indicates whether the required amount of ingredients has been added.
    /// </summary>
    public bool IngredientsUsedUp => _usedIngredients == maxIngredientAmount;

    // Tracks current amount of added ingredients
    private int _usedIngredients = 0;

    /// <summary>
    /// Initializes the second step of the dumpling minigame.
    /// </summary>
    public override void Execute()
    {
        // Reset ingredient counter at the start of the step
        _usedIngredients = 0;

        // Set default bowl appearance
        bowl.sprite = bowls[0];

        // Continue standard step initialization
        base.Execute();
    }

    /// <summary>
    /// Handles availability logic for interactions during this step.
    /// </summary>
    public override bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return IngredientsUsedUp;
    }

    /// <summary>
    /// Handles post-interaction logic depending on interacted object type.
    /// </summary>
    /// <param name="queryObject">
    /// Object responsible for triggering the interaction update.
    /// </param>
    public override void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        // If no specific object is provided, skip update logic
        if (!queryObject) return;

        // Update ingredient counter when an ingredient is added
        if (queryObject is IngredientManager)
            IngredientUsed();

        // Complete step when mixing action is performed after all ingredients are added
        else if (queryObject is SpoonController)
            SetCompleted();
    }

    /// <summary>
    /// Handles logic for when an ingredient is used during this step. Increments the ingredient counter and updates bowl visuals when all ingredients are added.
    /// </summary>
    private void IngredientUsed()
    {
        // Increment ingredient counter
        _usedIngredients++;

        // Update bowl appearance once all ingredients are added
        if (IngredientsUsedUp)
            bowl.sprite = bowls[1];
    }
}
