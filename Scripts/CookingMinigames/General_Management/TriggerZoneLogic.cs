using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls interaction logic for a minigame trigger zone. 
/// </summary>
///
/// Responsibilities:
/// - Detect player entering/exiting interaction area
/// - Store order/completion state for the zone
/// - Notify NPC order system about active interaction zone
/// - Launch associated minigame sequence

public class TriggerZoneLogic : MonoBehaviour
{
    /// <summary>
    /// Unique identifier for this interaction zone.
    /// </summary>
    [field: SerializeField] public int Index { get; private set; }

    // Background image displayed during minigame
    [SerializeField] private Image backgroundImage;

    // Sprite displayed during minigame, assigned when the minigame starts
    [SerializeField] private Sprite background;

    // Minigame controller associated with the particular minigame for this zone
    [SerializeField] private BaseMinigameManager minigameManager;

    /// <summary>
    /// Indicates whether the minigame completed for the current order in this zone.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Indicates whether customer ordered a dish that is associated with the minigame assigned to this zone.
    /// </summary>
    public bool Ordered { get; private set; } = false;

    private void OnEnable()
    {
        // Subscribes to the order controller's reset event to update this zone's state when a new order is ready.
        NPC_OrderController.ResetAfterOrderIsReady += SetCompleted;
        NPC_OrderController.ResetAfterOrderIsReady += SetOrdered;
    }

    private void OnDisable()
    {
        // Unsubscribes from the order controller's reset event to prevent memory leaks and unintended behavior when this zone is disabled.
        NPC_OrderController.ResetAfterOrderIsReady -= SetCompleted;
        NPC_OrderController.ResetAfterOrderIsReady -= SetOrdered;
    }

    /// <summary>
    /// Updates completion state of this zone.
    /// </summary>
    public void SetCompleted(bool state) => IsCompleted = state;

    /// <summary>
    /// Updates active order state of this zone.
    /// </summary>
    public void SetOrdered(bool state) => Ordered = state;

    /// <summary>
    /// Starts the assigned minigame and updates UI visuals.
    /// </summary>
    public void StartMinigame()
    {
        // Apply correct background sprite for the minigame
        backgroundImage.sprite = background;

        // Launch associated minigame flow
        minigameManager.StartMinigame();
    }

    /// <summary>
    /// Detects player entering the trigger zone. Registers this zone in the order controller if an active order exists, so the minigame can be launched when the player interacts.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Ordered)
                NPC_OrderController.Instance.SetupZone(this);
        }
    }

    /// <summary>
    /// Detects player leaving the interaction zone and clears active zone reference.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        NPC_OrderController.Instance.ResetZone();

    }
}
