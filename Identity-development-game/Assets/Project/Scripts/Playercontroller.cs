using UnityEngine;
using Terresquall;

/// <summary>
/// Player controller that moves a player GameObject using a virtual joystick,
/// </summary>
public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed;// 120-150 for normal movement
    [SerializeField] public int JID = 1;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject canvas;
    [SerializeField] public LayerMask interactableLayer = ~0; // default: everything
    private Rigidbody body;
    private Vector3 screenBoundsMin;
    private Vector3 screenBoundsMax;

    void Start(){
        body = player.GetComponent<Rigidbody>();
        SetScreenBounds();
    }

    void Update(){
        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);
        //Debug.Log($"Horizontal: {h}, Vertical: {v}");

        // move in world space using speed and delta time
        Vector3 delta = new Vector3(h * speed * Time.deltaTime, 0f, v * speed * Time.deltaTime);//x, y, z
        if (player != null){
            // Clamp the player inside the precomputed world bounds
            Vector3 deltaP = player.transform.position + delta;
            //deltaP.x = Mathf.Clamp(deltaP.x, screenBoundsMin.x, screenBoundsMax.x);
            //deltaP.y = Mathf.Clamp(deltaP.y, screenBoundsMin.y, screenBoundsMax.y);
            player.transform.position = deltaP;
        }

        // example usage: check nearby and get the GameObject
        InteractableObject nearbyIO;
        if (IsInteractableNearby(3f, out nearbyIO, interactableLayer)){
            Debug.Log("Nearby interactable: " + nearbyIO.gameObject.name);
        }
    }

    /*  
     * Get the world-space screen bounds at the object's z distance from the canvas background and
     * save them to screenBoundsMin and screenBoundsMax
     */
    public void SetScreenBounds(){
        Vector3[] worldCorners = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
        Vector3 bottomLeft = worldCorners[0]; 
        Vector3 topRight = worldCorners[2]; 
        screenBoundsMin = bottomLeft;
        screenBoundsMax = topRight;

        // screenBoundsMin  = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        // screenBoundsMax  = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));
        print("Screen LB: " + screenBoundsMin + " RU: " + screenBoundsMax);
    } 


    public void InteractWithObject(){
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if (hit.collider != null){
                Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<InteractableObject>()?.OnInteract();
            }
        }
    }

    /// <summary>
    /// Checks whether any InteractableObject exists within the given radius around the player.
    /// Returns true and the nearest InteractableObject if found.
    /// </summary>
    public bool IsInteractableNearby(float radius, out InteractableObject nearest, LayerMask? mask = null)
    {
        nearest = null;
        if (player == null) return false;

        Collider[] cols;
        if (mask.HasValue)
            cols = Physics.OverlapSphere(player.transform.position, radius, mask.Value);
        else
            cols = Physics.OverlapSphere(player.transform.position, radius);

        float bestDist = float.MaxValue;
        foreach (var c in cols)
        {
            if (c == null) continue;
            var io = c.GetComponent<InteractableObject>();
            if (io == null) continue;
            float d = Vector3.Distance(player.transform.position, c.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                nearest = io;
            }
        }

        return nearest != null;
    }

    /// <summary>
    /// Convenience getter that returns the nearest interactable GameObject within radius, or null.
    /// </summary>
    public GameObject GetNearestInteractableNearby(float radius)
    {
        if (IsInteractableNearby(radius, out InteractableObject io, interactableLayer))
            return io.gameObject;
        return null;
    }

    /// <summary>
    /// Checks whether an InteractableObject is roughly in front of the player within a cone.
    /// </summary>
    public bool IsInteractableInFront(float maxDistance, float maxAngleDegrees, out InteractableObject nearest, LayerMask? mask = null)
    {
        nearest = null;
        if (player == null) return false;

        // Use the same OverlapSphere to find candidates then filter by angle and distance
        Collider[] cols = mask.HasValue ? Physics.OverlapSphere(player.transform.position, maxDistance, mask.Value)
                                        : Physics.OverlapSphere(player.transform.position, maxDistance);

        float bestDist = float.MaxValue;
        Vector3 forward = player.transform.forward;
        foreach (var c in cols)
        {
            if (c == null) continue;
            var io = c.GetComponent<InteractableObject>();
            if (io == null) continue;
            Vector3 to = (c.transform.position - player.transform.position);
            float angle = Vector3.Angle(forward, to);
            if (angle > maxAngleDegrees) continue;
            float d = to.magnitude;
            if (d < bestDist)
            {
                bestDist = d;
                nearest = io;
            }
        }

        return nearest != null;
    }

    /// <summary>
    /// Convenience getter that returns the nearest interactable GameObject in front (within cone), or null.
    /// </summary>
    public GameObject GetNearestInteractableInFront(float maxDistance, float maxAngleDegrees)
    {
        if (IsInteractableInFront(maxDistance, maxAngleDegrees, out InteractableObject io, interactableLayer))
            return io.gameObject;
        return null;
    }
} 
