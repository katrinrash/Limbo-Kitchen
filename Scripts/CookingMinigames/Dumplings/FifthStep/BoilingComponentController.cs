using System.Collections;
using UnityEngine;

public class BoilingComponentController : InteractableObject 
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private string targetTag;

    private Vector3 _defaultPosition;

    #region UnityMethods

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
        stepManager.OnStepStarted += SetUp;
        stepManager.OnStepCompleted += Disable;
        enabled = false;
    }

    private void OnDestroy()
    {
        stepManager.OnStepStarted -= SetUp;
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    #region PreparationalMethods
    private void SetUp()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        transform.localPosition = _defaultPosition;
    }

    #endregion

    public override void OnRaycast()
    {
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

        else if (transform.localPosition != _defaultPosition)
        {
            transform.localPosition = _defaultPosition;
            enabled = false;
        }
    }

    private void Check()
    {
        if(Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            enabled = false;
            StartCoroutine(Boil());
        }
    }

    private IEnumerator Boil()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        AudioManager.Instance.PlayAddToWaterSound();
        stepManager.UpdateAfterUsed(this);
        gameObject.SetActive(false);
    }

}
