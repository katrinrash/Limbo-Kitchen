using UnityEngine;

/// <summary>
/// Base class for all interactable objects in the scene. Provides common interaction lifecycle and basic movement logic.
/// </summary>
///
/// Purpose:
/// - Serves as a parent class for interactive gameplay objects
/// - Defines standard interaction callbacks that can be used by external systems
/// - Provides shared movement behavior based to reduce code duplication across different object types
///

public class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// Provides movement logic for the chosen object.
    /// </summary>
    protected virtual void Move()
    {
        transform.position = RaycastUtility.GetPoint();
    }

    /// <summary>
    /// Called when this object is hit by a raycast interaction.
    /// </summary>
    public virtual void OnRaycast()
    {
    }

    /// <summary>
    /// Called when interaction with this object is completed.
    /// </summary>
    public virtual void OnCompleted()
    {
    }
}
