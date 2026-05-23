using UnityEngine;

/// <summary>
/// Manager for the dough filling interaction in the dumpling preparation minigame. 
/// Handles detecting spoon interactions, updating dough visuals, and communicating with the step manager to track progress.
/// </summary>
///
/// Flow:
/// Spoon enters trigger → Fill dough → Mark step progress → Reset on completion

public class DoughController : MonoBehaviour
{
    // Visual object representing dough filling
    [SerializeField] private GameObject filling;

    // Step manager controlling current gameplay stage
    [SerializeField] private BaseStepManager stepManager;

    // Tracks whether dough already has filling applied
    private bool _isFilled;

    #region UnityMethods

    private void Awake()
    {
        // Subscribe to step completion event to reset when the step is completed
        stepManager.OnStepCompleted += ResetComponent;
    }

    private void OnDestroy()
    {
        // Unsubscribe from step completion event
        stepManager.OnStepCompleted -= ResetComponent;
    }

    #endregion

    /// <summary>
    /// Resets dough to initial state
    /// </summary>
    public void ResetComponent()
    {
        // Hide filling visual and reset state
        filling.SetActive(false);
        _isFilled = false;
    }

    /// <summary>
    /// Detects spoon interaction and fills dough if conditions are met
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon") && !_isFilled && stepManager.GetAvailabilityInfo())
        {
            // Manage audio feedback for filling action
            AudioManager.Instance.PlayAddIngredientSound();

            // Show filling visual and update state
            filling.SetActive(true);
            _isFilled = true;

            // Notify step manager that interaction occurred and check for step completion
            stepManager.UpdateAfterUsed();
        }
    }
}
