using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GaiwanShakerManager : MonoBehaviour  
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private Transform endPos;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private int shakeLimit = 5;
    [SerializeField] private float shakePower;
    [SerializeField] private float duration = 1f;
    [SerializeField] float maxShakePower = 1.5f;
    [SerializeField] float decay = 1f;
    [SerializeField] float mouseToShakeMultiplier = 0.002f;

    private Vector3 _startPos;
    private int _currentElapsedShakes = 0;
    private bool _isShaking = false;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepStarted += Initialize;
        stepManager.OnStepCompleted += ResetGaiwan;
        _startPos = transform.position;
        enabled = false;
    }

    private void OnDestroy()
    {
        stepManager.OnStepStarted -= Initialize;
        stepManager.OnStepCompleted -= ResetGaiwan;
    }

    private void Update()
    {
        Calculate();

        if (InputManager.Instance.MouseAction.IsPressed() && Mouse.current.delta.ReadValue().magnitude > 0 && !_isShaking)
        {
            _isShaking = true;
            StartCoroutine(ShakeGaiwan());
        }
    }

    #endregion

    #region Preparation
    private void Initialize()
    {
        enabled = true;
    }

    private void ResetGaiwan()
    {
        StopAllCoroutines();
        transform.SetLocalPositionAndRotation(_startPos, Quaternion.identity);
        _isShaking = false;
        _currentElapsedShakes = 0;
    }
    #endregion

    private IEnumerator ShakeGaiwan()
    { 
        float elapsedTime = 0f;

        AudioManager.Instance.PlayShakeSound();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * shakePower;
            float t = elapsedTime / duration;
            transform.SetPositionAndRotation(Vector3.Lerp(_startPos, endPos.position, animationCurve.Evaluate(t)), Quaternion.Slerp(Quaternion.identity, endPos.rotation, animationCurve.Evaluate(t))); 
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * shakePower;
            float t = elapsedTime / duration;
            transform.SetPositionAndRotation(Vector3.Lerp(endPos.position, _startPos, animationCurve.Evaluate(t)), Quaternion.Slerp(endPos.rotation, Quaternion.identity, animationCurve.Evaluate(t)));
            yield return null;
        }

        _currentElapsedShakes++;

        if (_currentElapsedShakes >= shakeLimit)
        {
            stepManager.UpdateAfterUsed();
            enabled = false;
        }

        _isShaking = false;
    }

    private void Calculate()
    {
        float mouseSpeed = Mouse.current.delta.ReadValue().magnitude;
        shakePower += mouseSpeed * mouseToShakeMultiplier;
        shakePower = Mathf.Clamp(shakePower, 1f, maxShakePower);
        shakePower = Mathf.MoveTowards(shakePower, 1f, decay * Time.deltaTime);
    }

}

