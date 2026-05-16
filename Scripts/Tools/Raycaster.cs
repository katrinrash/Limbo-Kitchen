/// <summary>
/// Static helper class for performing raycast-based interaction checks.
/// Used to detect interactable objects and validate hits based on tags or components.
/// </summary>
///
/// Purpose:
/// - Centralize raycasting logic for interaction system
/// - Provide reusable methods for different hit validation cases
/// - Reduce duplication across input and gameplay scripts
///
/// Architecture:
/// - Static helper class (no instantiation required)
/// - Supports both tag-based and component-based detection

public static class Raycaster
{
    /// <summary>
    /// Checks if a raycast from a given start point hits an object with a specific tag.
    /// </summary>
    
    /// <param name="tag">Target tag to compare against.</param>
    /// <param name="startPoint">Ray origin point in world space.</param>
     
    /// <returns>True if a matching tagged object is hit, otherwise false.</returns>
    
    public static bool CheckRaycastResult(string tag, Vector3 startPoint)
    {
        // Cast ray forward from the given start point
        if (Physics.Raycast(startPoint, Vector3.forward, out RaycastHit hit))
        {
            // Safety check 
            if (hit.collider == null) return false;

            // Compare tag for interaction validation
            if (hit.collider.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if a raycast hits a specific InteractableObject instance.
    /// </summary>
    
    /// <param name="component">Target interactable object reference.</param>
    /// <param name="startPoint">Ray origin point in world space.</param>
    
    /// <returns>True if the exact component instance is hit.</returns>
    public static bool CheckRaycastResult(InteractableObject component, Vector3 startPoint)
    {
        // Cast ray forward from the given start point
        if (Physics.Raycast(startPoint, Vector3.forward, out RaycastHit hit))
        {
            // Safety check 
            if (hit.collider == null) return false;

            // Ensure the hit object has InteractableObject and matches reference
            if (hit.collider.gameObject.TryGetComponent(out InteractableObject comp) && comp == component)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns an InteractableObject hit by the given ray, if any exists.
    /// </summary>
     
    /// <param name="ray">Ray used for detection.</param>
     
    /// <returns>InteractableObject if found; otherwise null.</returns>
     
    public static InteractableObject GetRaycastResult(Ray ray)
    {
        // Perform raycast using the provided ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Safety check 
            if (hit.collider == null) return null;

            // Try to extract interactable component from hit object
            if (hit.collider.gameObject.TryGetComponent(out InteractableObject comp))
            {
                return comp;
            }
        }

        return null;
    }
}
