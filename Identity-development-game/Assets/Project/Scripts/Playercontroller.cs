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
        if (IsInteractableNearby(3f, out nearbyIO)){
            nearbyIO.OnInteract();
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
    } 


    public void InteractWithObject(){
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if (hit.collider != null){
                Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<InteractableObject>().OnInteract();
            }
        }
    }

    /// <summary>
    /// Checks whether any InteractableObject exists within the given radius around the player.
    /// Returns true and the nearest InteractableObject if found.
    /// </summary>
    public bool IsInteractableNearby(float radius, out InteractableObject nearest, LayerMask mask){
        nearest = null;
        if (player == null) return false;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius, mask);
        if (colliders.Length == 0) return false;

        float bestDist = float.MaxValue;
        Collider playerCollider = player.GetComponent<Collider>();
        foreach (var c in colliders){
            if (playerCollider != null && c == playerCollider) continue;
            var interactable = c.GetComponent<InteractableObject>();
            if (interactable == null) continue;
            float d = Vector3.Distance(player.transform.position, c.transform.position);

            if (d < bestDist){
                bestDist = d;
                nearest = interactable;
            }
        }
        return nearest != null;
    }

    // Backwards-compatible method: uses the serialized interactableLayer mask.
    public bool IsInteractableNearby(float radius, out InteractableObject nearest)
    {
        return IsInteractableNearby(radius, out nearest, interactableLayer);
    }

}