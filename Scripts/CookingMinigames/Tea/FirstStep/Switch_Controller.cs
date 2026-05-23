using UnityEngine;

public class Switch_Controller : InteractableObject
{
    [SerializeField] private BoilingIndicatorLogic boilingIndicator; 
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private Vector3 targetEulerAngles;

    private bool _stoveIsEmpty = true;
    private bool _isUsed = false;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += Disable;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    #region Raycast Logic

    public override void OnRaycast()
    {
        if (_stoveIsEmpty || _isUsed) return;
        
        transform.rotation = transform.rotation == Quaternion.identity ? Quaternion.Euler(targetEulerAngles) : Quaternion.identity;

        ApplyReaction();
    }

    private void ApplyReaction()
    {
        if (transform.rotation == Quaternion.identity)
        {
            boilingIndicator.ValidateSwitching();
            _isUsed = true;
            return;
        }

        boilingIndicator.StartProcess();
    }

    #endregion

    public void ChangeContent()
    {
        _stoveIsEmpty = !_stoveIsEmpty;
    }

    private void Disable()
    {
        transform.rotation = Quaternion.identity;
        _stoveIsEmpty = true;
        _isUsed = false;
    }
}
