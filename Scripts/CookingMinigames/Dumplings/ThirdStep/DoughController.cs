using UnityEngine;

// Manager for the dough filling interaction in the dumpling preparation minigame. 
// Handles detecting spoon interactions, updating dough visuals, and communicating with the step manager to track progress.
//
// Flow:
// Spoon enters trigger → Fill dough → Mark step progress → Reset on completion

public class DoughController : MonoBehaviour
{
    [SerializeField] private GameObject filling;
    [SerializeField] private BaseStepManager stepManager;

    private bool _isFilled;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += ResetComponent;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetComponent;
    }

    #endregion


    public void ResetComponent()
    {
        filling.SetActive(false);
        _isFilled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon") && !_isFilled && stepManager.GetAvailabilityInfo())
        {

            AudioManager.Instance.PlayAddIngredientSound();

            filling.SetActive(true);
            _isFilled = true;

            stepManager.UpdateAfterUsed();
        }
    }
}
