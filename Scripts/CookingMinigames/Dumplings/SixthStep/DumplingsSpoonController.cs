using UnityEngine;

public class DumplingsSpoonController : BaseFillingSpoonController
{
    [SerializeField] private PlateManager _plateManager;
    [SerializeField] private int componentsToAddCount;

    public bool IsAvailable { get; private set; } = true;

    private int _dumplingsUsed;
    private GameObject _dumpling;

    private void Update()
    {
        if (_dumplingsUsed >= componentsToAddCount)
        {
            IsAvailable = false;
            stepManager.UpdateAfterUsed(this);
            base.Disable();
            return;
        }

        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        else if (transform.position != _defaultPosition)
        {
            transform.position = _defaultPosition;
            enabled = false;
        }
 
    }

    public override void OnRaycast()
    {  
        if(!IsAvailable) return;
        
        enabled = true;
    }

    protected override void Check()
    {
        if (Raycaster.CheckRaycastResult(_plateManager, transform.position) && IsFilled) 
        {
            _plateManager.AddDumplings(_dumpling);
            UpdateState();
            _dumplingsUsed++;
        }
    }

    public void TakeDumpling(GameObject dumpl)
    { 
        _dumpling = dumpl;
        IsFilled = true;
        spoonSpriteRenderer.sprite = spoonSprites[1];
    }



    protected override void Disable()
    {
        base.Disable();
        _dumplingsUsed = 0;
        IsAvailable = true;
    }
}
