using UnityEngine;

public class DumplingPickup : MonoBehaviour
{
    [SerializeField] private BaseStepManager stepManager;
    
    private bool _isUsed;
    private Vector3 _startPos;

    #region UnityMethods

    private void Awake()
    {
        _startPos = transform.position;
        stepManager.OnStepStarted += Initialize;
        stepManager.OnStepCompleted += Disable;
    }

    private void OnDestroy()
    {
        stepManager.OnStepStarted -= Initialize;
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    #region PreparationalMethods
    private void Initialize()
    {
        gameObject.SetActive(true);
        _isUsed = false;
    }

    private void Disable()
    {
        transform.position = _startPos;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon") && !_isUsed)
        {
            DumplingsSpoonController spoon = other.gameObject.GetComponent<DumplingsSpoonController>();

            if (!spoon || spoon.IsFilled) return;

            gameObject.SetActive(false);
            _isUsed = true;
            spoon.TakeDumpling(gameObject);
        }
    }
}
