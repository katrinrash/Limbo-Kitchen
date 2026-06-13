using UnityEngine;
using UnityEngine.InputSystem;

// Executable for the second step of the dumpling preparation minigame. 
//
// Flow:
// Select spot → Fold dumpling → Update state → Repeat until completion

public class DumplingsFourthStepExecutable : BaseStepManager
{
    [SerializeField] private GameObject[] spots;
    [SerializeField] private SpriteRenderer dumplingSpriteRenderer;
    [SerializeField] private Sprite[] dumplingStates;
    [SerializeField] private string spotTag;

    private GameObject _currentSpot;
    private int _currentStateIndex;


    public override void Execute()
    {
        _currentStateIndex = 0;
        _currentSpot = spots[_currentStateIndex];
        dumplingSpriteRenderer.sprite = dumplingStates[_currentStateIndex];

        _currentSpot.SetActive(true);

        base.Execute();
    }

    protected override void Check(InputAction.CallbackContext context)
    {
        if (Raycaster.CheckRaycastResult(spotTag, RaycastUtility.GetPoint()))
        {
            _currentSpot.SetActive(false);

            AudioManager.Instance.PlayFoldSound();

            dumplingSpriteRenderer.sprite = dumplingStates[++_currentStateIndex];

            if (_currentStateIndex > spots.Length - 1)
            {
                SetCompleted();
                return;
            }

            _currentSpot = spots[_currentStateIndex];
            _currentSpot.SetActive(true);
        }
    }
}
