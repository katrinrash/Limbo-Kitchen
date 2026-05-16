using UnityEngine;
using UnityEngine.UI;

public class LastStep : MonoBehaviour, IExecutable
{
    [SerializeField] protected Sprite[] results;
    [SerializeField] private Image result;

    public IStateHandler StateHandler { get; set; }

    public void Execute()
    {
        SwitchBTNLogic.Instance.RequirementsCompleted(this);
        result.sprite = results[OrderValidator.Instance.GetDishResultIndex()];
        result.gameObject.SetActive(true);
        AudioManager.Instance.PlayResult(OrderValidator.Instance.GetDishResultIndex());

        gameObject.SetActive(true);
    }

    public void EndExecution()
    {
        result.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
