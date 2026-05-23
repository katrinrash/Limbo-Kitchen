using UnityEngine;

public class TeabagController : InteractableObject 
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private GaiwanManager gaiwan;
    [SerializeField] private SpicesEnum _preferredTeaForTheType;
    [SerializeField] private float teaPrice = 0.5f;
    [SerializeField] private Sprite mainSprite;
    [SerializeField] private SpriteRenderer mainRenderer;

    private Vector3 _startPos;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += ResetTeaBag;
        _startPos = transform.position;
        enabled = false;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetTeaBag;
    }

    #endregion

    public override void OnRaycast()
    {
        if (stepManager.GetAvailabilityInfo()) return;

        mainRenderer.sprite = mainSprite;
        enabled = true;
    }

    void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        if (transform.localPosition != _startPos)
        {
            mainRenderer.sprite = null;
            transform.position = _startPos;
            enabled = false;
        }
    }

    private void Check()
    {
        if (Raycaster.CheckRaycastResult(gaiwan, transform.position))
        {
            OnCompleted();
        }
    }

    public override void OnCompleted()
    {
        enabled = false;
        AudioManager.Instance.AddTeaSound();
        stepManager.UpdateAfterUsed();
        CurrencyManager.Instance.UseCurrency(teaPrice);
        mainRenderer.sprite = null;
        OrderValidator.Instance.ValidateSpices(_preferredTeaForTheType);
    }

    public void ResetTeaBag()
    {
        transform.position = _startPos;
        mainRenderer.sprite = null;
    }
}
