using System.Collections;
using UnityEngine;

// Controls dough rolling interaction for the dumpling minigame. Handles rolling animation, dough state progression, and completion logic.
//
// Flow:
// Player interaction → Roller animation → Dough state update → Repeat until step completion

public class RollerManager : InteractableObject
{
    [Header("Base Data")]

    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private SpriteRenderer doughSpriteRenderer;
    [SerializeField] private Sprite[] doughStates;

    [Header("Animation Details")]

    [SerializeField] private Vector3 upPosition;
    [SerializeField] private Vector3 downPosition;
    [SerializeField] private float _rollingSpeed = 1f;

    private int CurrentIteration = 0;
    private int _maxAmountOfIterations = 0;
    private bool _isWorking = false;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += ResetComponent;
        _maxAmountOfIterations = doughStates.Length - 1;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= ResetComponent;
    }

    #endregion

    public override void OnRaycast()
    {
        if (_isWorking || CurrentIteration == _maxAmountOfIterations)
            return;

        StartCoroutine(MoveRollerCoroutine());
        AudioManager.Instance.PlayDoughSound();
    }

    private IEnumerator MoveRollerCoroutine()
    {
        _isWorking = true;

        var startPosition = transform.localPosition;
        var targetPosition = transform.localPosition == upPosition ? downPosition : upPosition;

        float positionIndex = 0f;

        while (transform.localPosition != targetPosition)
        {
            positionIndex += Time.deltaTime * _rollingSpeed;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, positionIndex);

            yield return null;
        }

        UpdateDoughState();

        _isWorking = false;
    }

    private void UpdateDoughState()
    {
        doughSpriteRenderer.sprite = doughStates[++CurrentIteration];

        if (CurrentIteration == _maxAmountOfIterations)
        {
            stepManager.UpdateAfterUsed();
            return;
        }
    }

    private void ResetComponent()
    {
        StopAllCoroutines();
        CurrentIteration = 0;

        doughSpriteRenderer.sprite = doughStates[CurrentIteration];
        transform.localPosition = downPosition;

        _isWorking = false;
    }
}
