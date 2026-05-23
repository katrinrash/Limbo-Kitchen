using UnityEngine;

public class GaiwanManager : InteractableObject
{
    [SerializeField] protected BaseStepManager stepManager;

    #region UnityMethods

    protected virtual void Awake()
    {
        stepManager.OnStepStarted += Initialize;
    }

    protected virtual void OnDestroy()
    {
        stepManager.OnStepStarted -= Initialize;
    }

    #endregion

    public bool IsClosed { get; protected set; }
    public bool IsEmpty { get; protected set; }


    protected virtual void Initialize()
    { 
        IsClosed = false;
        IsEmpty = true;
    }

    public void ChangeAvailability()
    {
        IsClosed = !IsClosed;
    }

    public void ChangeTheContent()
    { 
        IsEmpty = !IsEmpty;
        OnContentChanged();
    }

    protected virtual void OnContentChanged()
    {
        stepManager.UpdateAfterUsed();
    }
}
