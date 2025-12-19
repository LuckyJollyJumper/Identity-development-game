using UnityEngine;
using Terresquall;

/// <summary>
/// Player controller that moves a player GameObject using a virtual joystick,
/// </summary>
public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed;// 120-150 for normal movement
    [SerializeField] public int JID = 1;//ID of Joystick
    [SerializeField] public GameObject player;
    [SerializeField] public LayerMask interactableLayer = ~0; // default: everything
    public InteractableObject currentInteractableObject;
    private Rigidbody body;
    private enum InteractionState{Idle, Searching, FoundObject};
    private InteractionState playerState;

    void Start(){
        this.body = player.GetComponent<Rigidbody>();
        this.playerState = InteractionState.Searching;
    }

    void Update(){
        // Move in world space using speed and delta time
        MovePlayer();
        // Check for object in proximity and activate them
        ActivateSurroundingObjects();
    }

    public void MovePlayer(){
        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);

        Vector3 delta = new Vector3(h * speed * Time.deltaTime, 0f, v * speed * Time.deltaTime);//x, y, z
        if (player != null){
            Vector3 deltaP = player.transform.position + delta;
            player.transform.position = deltaP;
        }
    }


    public void ActivateSurroundingObjects(){
        if (IsInteractableNearby(3f, out InteractableObject nearbyIO)){
            // Only used on initial discovery of object
            if (playerState == InteractionState.Searching){
                playerState = InteractionState.FoundObject;
                this.currentInteractableObject = nearbyIO;
                nearbyIO.SetInteractionState(InteractableObject.ObjectState.ReadyForInteraction);
                Debug.Log("Found interactable: " + nearbyIO.gameObject.name);
            }
        }
        // Used to close object if player walks out of proximity
        else if (playerState == InteractionState.FoundObject){ 
            currentInteractableObject.SetInteractionState(InteractableObject.ObjectState.Idle);
            playerState = InteractionState.Searching;     
            Debug.Log("Lost interactable: " + currentInteractableObject.gameObject.name);   
            currentInteractableObject = null;    
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
    public bool IsInteractableNearby(float radius, out InteractableObject nearest){
        return IsInteractableNearby(radius, out nearest, interactableLayer);
    }

    public void InteractWithObject(){
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (Physics.Raycast(ray, out RaycastHit hit)){
            if (hit.collider != null){
                Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<InteractableObject>().OnInteract();
            }
        }
    }


    public void RayCastFromTouch(Vector2 touchPos){
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (Physics.Raycast(ray, out RaycastHit hit)){
            if (hit.collider != null){
                Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<InteractableObject>().OnInteract();
            }
        }
    }
}