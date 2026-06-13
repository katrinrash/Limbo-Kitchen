using System.Collections;
using UnityEngine;

// Base interactable ingredient used in the minigames. 
//
// Responsibilities:
// - Allow player to pick and drag ingredient
// - Validate correct placement using raycast checks
// - Trigger ingredient usage logic
// - Animate ingredient usage
// - Reset state after step completion

public class IngredientManager : InteractableObject
{
    [Header("Base Data")]

    [SerializeField] protected BaseStepManager stepManager;

    [Header("Animation Data")]

    [SerializeField] protected AnimationCurve animationCurve;
    [SerializeField] protected float animationDuration = 1f;

    [Header("Ingredient Data")]

    [SerializeField] protected PreferencesEnum ingredientType;
    [SerializeField] protected int index;
    [SerializeField] protected float ingredientPrice = 0.3f;
    [SerializeField] protected string targetTag;

    protected bool _isAvailable = true;
    protected Vector3 _defaultPosition;

    #region UnityMethods

    protected virtual void Awake()
    {
        _defaultPosition = transform.position;
        stepManager.OnStepCompleted += ResetIngredient;
        enabled = false;
    }

    protected virtual void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetIngredient;
    }

    #endregion

    private void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed() && _isAvailable)
        {
            Move();
            Check();
            return;
        }

        else if (transform.position != _defaultPosition)
        {
            HandlePositionReset();
        }
    }

    public override void OnRaycast()
    {
        if (!_isAvailable || stepManager.GetAvailabilityInfo())
            return;

        HandleIngredientChosen();
    }

    protected virtual void HandleIngredientChosen()
    {
        enabled = true;
    }

    protected virtual void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            HandleRaycast();
        }
    }

    protected virtual void HandleRaycast()
    {

        _isAvailable = false;
        enabled = false;

        AudioManager.Instance.PlayAddIngredientSound();
        StartCoroutine(UseIngredient());
    }

    protected virtual IEnumerator UseIngredient()
    {
        OrderValidator.Instance.ValidateIngredient(ingredientType, index);
        stepManager.UpdateAfterUsed(this);
        CurrencyManager.Instance.UseCurrency(ingredientPrice);

        var startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(startPosition, _defaultPosition, animationCurve.Evaluate(t));

            yield return null;
        }
    }

    protected virtual void HandlePositionReset()
    {
        transform.position = _defaultPosition;
        enabled = false;
    }

    protected virtual void ResetIngredient()
    {
        StopAllCoroutines();

        transform.SetPositionAndRotation(_defaultPosition, Quaternion.identity);

        _isAvailable = true;
        enabled = false;
    }
}
