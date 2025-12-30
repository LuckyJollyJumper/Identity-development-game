using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;

/// <summary>
/// Player controller that moves a player GameObject using a virtual joystick,
/// </summary>
public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed;// 120-150 for normal movement
    [SerializeField] public int JID = 1;//ID of Joystick
    [SerializeField] public GameObject player;
    [SerializeField] public float floorHeight = 0.03f;
    [SerializeField] public LayerMask interactableLayer = ~0; // default: everything
    public Dictionary<InteractableObject, float> currentInteractableObjects;
    public float interactionRadius = 0.5f;
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

        MouseInteract();
    }

    public void MovePlayer(){
        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);

        Vector3 delta = new Vector3(h * speed * Time.deltaTime, 0f, v * speed * Time.deltaTime);//x, y, z
        if (player != null){
            Vector3 deltaP = player.transform.position + delta;
            player.transform.position = deltaP;
        }

        // Move player rotation to face movement direction
        Vector3 direction = new Vector3(h, 0f, v);
        if (direction.magnitude > 0.1f){
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    // Mistake is that if 2 objects are close the leave trigger does not 
    // do anything and objects do not get closed and stay interactable until all objects are gone
    public void ActivateSurroundingObjects(){
        if (IsInteractableNearby(interactionRadius+0.5f, out Dictionary<InteractableObject, float> nearbyIO, interactableLayer)){
            // Only used on initial discovery of object
            foreach (var io in nearbyIO){
                // If within interaction radius and currently idle, set to ready for interaction (Ignore if already ready or interacting)
                if (io.Value <= interactionRadius && io.Key.currentState == InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.ReadyForInteraction);
                    Debug.Log("Found interactable: " + io.Key.gameObject.name);
                }else if (io.Value > interactionRadius && io.Key.currentState != InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.Idle);
                    Debug.Log("Closing interactable: " + io.Key.gameObject.name);
                }
            }
           
        }
    }


    /// <summary>
    /// Checks whether any InteractableObject exists within the given radius around the player.
    /// Returns true and the all InteractableObjects with distance if found.
    /// </summary>
    public bool IsInteractableNearby(float radius, out Dictionary<InteractableObject, float> foundObjects, LayerMask mask){
        foundObjects = new Dictionary<InteractableObject, float>();
        if (player == null) return false;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius, mask);
        if (colliders.Length == 0) return false;

        Collider playerCollider = player.GetComponent<Collider>();
        foreach (var c in colliders){
            if (playerCollider != null && c == playerCollider) continue;
            var interactable = c.GetComponent<InteractableObject>();
            if (interactable == null) continue;
            float d = Vector3.Distance(player.transform.position, c.transform.position);

            foundObjects.Add(interactable, d);
        }
        return foundObjects.Count > 0;
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


    public void MouseInteract(){
//         if (Input.touchCount > 0){
//             Touch touch = Input.GetTouch(0); // Get the first touch
//             Vector3 touchPosition3D = Camera.main.ScreenToWorldPoint(touch.position); // for 3d games

//             if (Physics.Raycast(rayOrigin, out RaycastHit hitInfo))  {
//                 GameObject GO = hitInfo.collider.gameObject;
//                 MeshRenderer renderer = GO.GetComponent<MeshRenderer>();
//                 if (renderer != null)
//                 {
//                     renderer.material.color = new Color(Random.value, Random.value, Random.value);
//                 }
// ;
//             }
//         }
    }

    // Draw only the interaction radius sphere in the editor for debugging
    void OnDrawGizmosSelected(){
        if (player == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, interactionRadius);
    }

}