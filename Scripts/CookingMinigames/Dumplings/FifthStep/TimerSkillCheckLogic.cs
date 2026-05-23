using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimerSkillCheckLogic : MonoBehaviour
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private float xStart;
    [SerializeField] private float xEnd;
    [SerializeField] private float minThreshold;
    [SerializeField] private float maxThreshold;
    [SerializeField] private float time = 1f;
    [SerializeField] private float normalSpeed = 1f;
    [SerializeField] private float peakHourSpeed = 1.25f;
    [SerializeField] private int skillChecksAmount;
    [SerializeField] private GameObject parent;

    public bool SkillCheckValid => _providedSkillChecks < skillChecksAmount;

    private Vector3 _start;
    private Vector3 _end;
    private float _position;
    private float _currentSpeed;
    private int _providedSkillChecks;

    #region UnityMethods

    private void Awake()
    {
        _start = new Vector3(xStart, -0.23f, -0.2f);
        _end = new Vector3(xEnd, -0.23f, -0.2f);
        stepManager.OnStepCompleted += Disable;
    }

    private void Start()
    {
        parent.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.Instance.ValidationAction.performed += ReactToTheInput;
    }

    private void OnDisable()
    {
        InputManager.Instance.ValidationAction.performed -= ReactToTheInput;
    }
    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= Disable;
    }

    #endregion

    public void Initialize()
    { 
        parent.SetActive(true);
        transform.position = _start;
        _providedSkillChecks = 0;
        _position = 0;
        _currentSpeed = ShiftManager.Instance.IsPeakHour ? peakHourSpeed : normalSpeed;
        Debug.Log(_currentSpeed);
        enabled = true;
        StartCoroutine(StartSkillCheck());
    }

    private void ReactToTheInput(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        transform.localPosition = _start;
        CheckResult();
    }

    private void CheckResult()
    {
        if (_position >= minThreshold && _position <= maxThreshold)
        { 
            stepManager.UpdateAfterUsed(this);
        }

        _providedSkillChecks++;
        SetUpTimer();
    }

    private void SetUpTimer()
    {
        if (_providedSkillChecks >= skillChecksAmount)
        {
            stepManager.UpdateAfterUsed(this);
            StopAllCoroutines();
            enabled = false;
            return;
        }

        StartCoroutine(StartSkillCheck());
    }

    private IEnumerator StartSkillCheck()
    {
        while (true)
        {
            _position = 0f;

            while (_position < time)
            {
                _position += Time.deltaTime * _currentSpeed;
                transform.localPosition = Vector3.Lerp(_start, _end, _position);
                yield return null;
            }

            _position = 0f;

            while (_position < time)
            {
                _position += Time.deltaTime * _currentSpeed;
                transform.localPosition = Vector3.Lerp(_end, _start, _position);
                yield return null;
            }

        }

    }

    private void Disable()
    {
        parent.SetActive(false);
        StopAllCoroutines();
    }
}
