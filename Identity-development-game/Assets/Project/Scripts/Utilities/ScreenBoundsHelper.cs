using UnityEngine;

/// <summary>
/// Utilities to compute world-space screen bounds and clamp positions.
/// Use ScreenToWorldPoint at a given Z distance from the camera (distance from camera) to get the corners.
/// </summary>
public static class ScreenBoundsHelper
{
    // Returns world-space min and max corners at the given zDistance (distance from the camera)
    public static void GetWorldBounds(Camera cam, float zDistance, out Vector3 min, out Vector3 max)
    {
        if (cam == null)
        {
            min = Vector3.zero;
            max = Vector3.zero;
            return;
        }

        min = cam.ScreenToWorldPoint(new Vector3(0f, 0f, zDistance));
        max = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));
    }

    // Clamp a world-space position inside the camera view at the given zDistance
    public static Vector3 ClampPositionToCameraView(Vector3 worldPos, Camera cam, float zDistance)
    {
        GetWorldBounds(cam, zDistance, out Vector3 min, out Vector3 max);
        worldPos.x = Mathf.Clamp(worldPos.x, min.x, max.x);
        worldPos.y = Mathf.Clamp(worldPos.y, min.y, max.y);
        return worldPos;
    }
}
