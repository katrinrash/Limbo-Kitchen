using System.Collections;
using UnityEngine;

public class PlateManager : InteractableObject
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private float animationDuration;
    [SerializeField] private Transform[] position;
    [SerializeField] private float multiplier;

    private GameObject _currentDumpling;
    private int _index = 0;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += ResetPlate;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetPlate;
    }

    #endregion

    public void AddDumplings(GameObject dumpl)
    {
        _currentDumpling = dumpl;
        StartCoroutine(AddDumpling());
    }

    private void ResetPlate()
    {
        _index = 0;
        StopAllCoroutines();
    }

    private IEnumerator AddDumpling()
    {
        Transform dumpl = _currentDumpling.transform;
        Vector3 start = position[_index].position + Vector3.up * multiplier;
        Vector3 end = position[_index++].position;
        dumpl.position = start;
        dumpl.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            dumpl.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

}
