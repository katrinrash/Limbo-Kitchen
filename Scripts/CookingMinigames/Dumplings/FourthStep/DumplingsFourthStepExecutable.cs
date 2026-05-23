using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Executable for the second step of the dumpling preparation minigame. 
/// </summary>
///
/// Flow:
/// Select spot → Fold dumpling → Update state → Repeat until completion

public class DumplingsFourthStepExecutable : BaseStepManager
{
    // Interaction points used to highlight folding spots during the step
    [SerializeField] private GameObject[] spots;

    // Reference to the sprite renderer used to update visuals
    [SerializeField] private SpriteRenderer dumplingSpriteRenderer;

    // Array of dumpling visual states during folding process
    [SerializeField] private Sprite[] dumplingStates;

    // Tag used to validate correct folding interaction
    [SerializeField] private string spotTag;

    // Currently active folding spot
    private GameObject _currentSpot;

    // Index of current folding state
    private int _currentStateIndex;

    /// <summary>
    /// Initializes the fourth step of the dumpling minigame.
    /// </summary>
    public override void Execute()
    {
        // Reset folding state index at the start of the step
        _currentStateIndex = 0;

        // Set the first folding spot as active for player interaction
        _currentSpot = spots[_currentStateIndex];

        // Set initial dumpling appearance
        dumplingSpriteRenderer.sprite = dumplingStates[_currentStateIndex];

        // Activate the first folding spot to guide player interaction
        _currentSpot.SetActive(true);

        // Continue standard step initialization
        base.Execute();
    }

    /// <summary>
    /// Handles validation of folding interactions and updates the dumpling state accordingly.
    /// </summary>
    protected override void Check(InputAction.CallbackContext context)
    {
        if (Raycaster.CheckRaycastResult(spotTag, RaycastUtility.GetPoint()))
        {
            // Deactivate current folding spot after successful interaction
            _currentSpot.SetActive(false);

            // Manage audio feedback for folding action
            AudioManager.Instance.PlayFoldSound();

            // Update dumpling sprite to reflect current folding state
            dumplingSpriteRenderer.sprite = dumplingStates[++_currentStateIndex];

            // Check if folding sequence is completed
            if (_currentStateIndex > spots.Length - 1)
            {
                // Mark step as completed when all folding spots have been interacted with
                SetCompleted();
                return;
            }

            // Activate next folding spot
            _currentSpot = spots[_currentStateIndex];
            _currentSpot.SetActive(true);
        }
    }
}
