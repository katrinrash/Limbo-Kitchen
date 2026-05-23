using System.Collections;
using UnityEngine;

public class RemoveContainerController : ContainerBaseController
{
    [Header("Remove Specific Data")]
    
    [SerializeField] private Transform targetPos;
    [SerializeField] private float animationDuration;
    [SerializeField] private IndicatorBaseLogic indicator;

    public bool IsReady { get; private set; } = false;

    #region UnityMethods

    protected override void Awake()
    {
        base.Awake();
        indicator.OnCompleted += Activate;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        indicator.OnCompleted -= Activate;
    }

    #endregion

    public void AddWater()
    { 
        IsReady = false;
    }

    protected override void ResetContainer()
    {
        base.ResetContainer();
        IsReady = false;
    }

    private void Activate()
    {
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(_startPos, targetPos.position, t);
            yield return null;
        }

        IsReady = true;
    }

}
