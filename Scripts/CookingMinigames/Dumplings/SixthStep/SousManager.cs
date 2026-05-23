using System.Collections;
using UnityEngine;

public class SousManager : InteractableObject
{
    [Header("General Data")]
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private int index;
    [SerializeField] private SpicesEnum _preferredSousForTheType;
    [SerializeField] private float sousPrice = 0.3f;

    [Header("Animation")]
    [SerializeField] private float animationDuration;
    [SerializeField] private Transform endPosition;
    [SerializeField] private AnimationCurve animationCurve;


    private Vector3 _startPos;

    #region UnityMethods

    private void Awake()
    {
        _startPos = transform.position;
        stepManager.OnStepCompleted += ResetSous;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetSous;
    }

    #endregion

    public override void OnRaycast()
    {
        if (!stepManager.GetAvailabilityInfo(this)) return;
        
        StartCoroutine(UseSous());
        CurrencyManager.Instance.UseCurrency(sousPrice);
    }

    private void ResetSous()
    { 
        StopAllCoroutines();
        transform.position = _startPos;
    }

    private IEnumerator UseSous()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(_startPos, endPosition.position, animationCurve.Evaluate(t));
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(endPosition.position, _startPos, animationCurve.Evaluate(t));
            yield return null;
        }

        OrderValidator.Instance.ValidateSpices(_preferredSousForTheType, index);
        stepManager.UpdateAfterUsed(this);
    }

}
