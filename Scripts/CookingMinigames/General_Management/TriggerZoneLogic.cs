using UnityEngine;
using UnityEngine.UI;

public class TriggerZoneLogic : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite background;
    [SerializeField] private BaseMinigameManager minigameManager;

    public bool IsCompleted {  get; private set; }

    public bool Ordered { get; private set; } = false; 

    private void OnEnable()
    {
        NPC_OrderController.ResetAfterOrderIsReady += SetCompleted;
        NPC_OrderController.ResetAfterOrderIsReady += SetOrdered;
    }

    private void OnDisable()
    {
        NPC_OrderController.ResetAfterOrderIsReady -= SetCompleted;
        NPC_OrderController.ResetAfterOrderIsReady -= SetOrdered;
    }

    public void SetCompleted(bool state) =>  IsCompleted = state; 
    public void SetOrdered(bool state) => Ordered = state;  

    public void StartMinigame()
    {
        backgroundImage.sprite = background;
        minigameManager.StartMinigame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Ordered)
                NPC_OrderController.Instance.SetupZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPC_OrderController.Instance.ResetZone();
        Debug.Log("Reset");
    }
}
