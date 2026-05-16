using System;
using UnityEngine;

public class BaseStepManager : MonoBehaviour, IExecutable
{
    public Action OnStepStarted;
    public Action OnStepCompleted;

    [field: SerializeField] public GameObject StageSetUp { get; protected set; }
    public IStateHandler StateHandler { get; set; }

    #region UnityMethods

    protected virtual void OnEnable()
    {
        InputManager.Instance.MouseAction.performed += Check;
    }

    protected virtual void OnDisable()
    {
        InputManager.Instance.MouseAction.performed -= Check;
        StageSetUp.SetActive(false);
    }

    #endregion

    protected virtual void Check(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        InteractableObject raycastResult = Raycaster.GetRaycastResult(RaycastUtility.GetRay());

        if (raycastResult)
            raycastResult.OnRaycast();
    }

    public virtual void UpdateBeforeUsed(MonoBehaviour queryObject = null)
    {
    }

    public virtual void UpdateAfterUsed(MonoBehaviour queryObject = null)
    {
        SetCompleted();
    }

    protected virtual void SetCompleted()
    {
        SwitchBTNLogic.Instance.RequirementsCompleted(this);
    }

    public virtual bool GetAvailabilityInfo(InteractableObject queryObject = null)
    {
        return false;
    }

    #region Execution
    public virtual void Execute()
    {
        StageSetUp.SetActive(true);
        gameObject.SetActive(true);

        OnStepStarted?.Invoke();
    }

    public virtual void EndExecution()
    {
        OnStepCompleted?.Invoke();
        gameObject.SetActive(false);
    }
    #endregion
}
