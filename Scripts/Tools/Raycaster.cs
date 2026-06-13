
// Static helper class for performing raycast-based interaction checks.
// Used to detect interactable objects and validate hits based on tags or components.
//
// Purpose:
// - Centralize raycasting logic for interaction system
// - Provide reusable methods for different hit validation cases
// - Reduce duplication across input and gameplay scripts
//
// Architecture:
// - Static helper class (no instantiation required)
// - Supports both tag-based and component-based detection

public static class Raycaster
{

    public static bool CheckRaycastResult(string tag, Vector3 startPoint)
    {
        if (Physics.Raycast(startPoint, Vector3.forward, out RaycastHit hit))
        {
            if (hit.collider == null) return false;

            if (hit.collider.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CheckRaycastResult(InteractableObject component, Vector3 startPoint)
    {
        if (Physics.Raycast(startPoint, Vector3.forward, out RaycastHit hit))
        {
            if (hit.collider == null) return false;

            if (hit.collider.gameObject.TryGetComponent(out InteractableObject comp) && comp == component)
            {
                return true;
            }
        }

        return false;
    }
 
    public static InteractableObject GetRaycastResult(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider == null) return null;

            if (hit.collider.gameObject.TryGetComponent(out InteractableObject comp))
            {
                return comp;
            }
        }

        return null;
    }
}
