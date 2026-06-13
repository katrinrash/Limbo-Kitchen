using UnityEngine;

// Base class for all interactable objects in the scene. Provides common interaction lifecycle and basic movement logic.
public class InteractableObject : MonoBehaviour
{
    protected virtual void Move()
    {
        transform.position = RaycastUtility.GetPoint();
    }

    public virtual void OnRaycast()
    {
    }

    public virtual void OnCompleted()
    {
    }
}
