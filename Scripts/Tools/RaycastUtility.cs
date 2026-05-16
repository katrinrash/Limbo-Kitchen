using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Utility class for converting mouse input into world-space rays and positions. Designed to simplify raycasting logic across the project.
/// </summary>
///
/// Use cases:
/// - Detecting clicks on game objects
/// - Converting mouse position into world coordinates to move objects 
///
/// Architecture:
/// - Static helper class (no instantiation required)
/// - Uses Unity's new Input System
/// - Relies on main camera for screen-to-world conversion

public static class RaycastUtility
{
    // Fixed Z-depth used for world-space point conversion
    // This defines how far into the scene the projected point is placed
    private static readonly float Z = 12f;

    /// <summary>
    /// Creates a ray from the current mouse position in screen space into the world using the main camera.
    /// </summary>
    
    /// <returns>Ray originating from camera through mouse position.</returns>
    
    public static Ray GetRay()
    {
        // Current mouse position in screen coordinates
        var mousePosition = Mouse.current.position.ReadValue();

        // Convert screen point into a world-space ray
        return Camera.main.ScreenPointToRay(mousePosition);
    }

    /// <summary>
    /// Converts the current mouse position into a world-space point.
    /// </summary>
    
    /// <returns>World-space position based on mouse cursor.</returns>
    
    public static Vector3 GetPoint()
    {
        // Read current mouse position
        var mousePosition = Mouse.current.position.ReadValue();

        // Convert screen position into world space
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        // Apply fixed depth offset and return the resulting world position
        return new Vector3(mousePosition.x, mousePosition.y, Z);
    }
}
