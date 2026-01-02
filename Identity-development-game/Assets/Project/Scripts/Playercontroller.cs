using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Player controller that moves a player GameObject using a virtual joystick,
/// </summary>
public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed;// 120-150 for normal movement
    [SerializeField] public float interactionRadius = 0.5f;
    [SerializeField] public LayerMask interactableLayer = ~0; // default: everything

    [Header("References")]
    [SerializeField] public int JID = 1;//ID of Joystick
    [SerializeField] public GameObject player;
    
    private Rigidbody body;
    private enum InteractionState{Idle, Searching, Interacting};
    private InteractionState playerState;
    private string DebugID = "[PlayerController]";

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


    /// <summary>
    /// Activates nearby InteractableObjects within interactionRadius. Calls ActivateSurroundingObjects with higher radius to
    /// detect objects entering/leaving interaction range. 
    /// </summary>
    public void ActivateSurroundingObjects(){
        if (IsInteractableNearby(interactionRadius+0.5f, out Dictionary<InteractableObject, float> nearbyIO, interactableLayer)){
            // Only used on initial discovery of object
            foreach (var io in nearbyIO){
                // If within interaction radius and currently idle, set to ready for interaction (Ignore if already ready or interacting)
                if (io.Value <= interactionRadius && io.Key.currentState == InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.ReadyForInteraction);
                    Debug.Log($"{DebugID} Found interactable: {io.Key.gameObject.name}");
                }else if (io.Value > interactionRadius && io.Key.currentState != InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.Idle);
                    Debug.Log($"{DebugID} Closing interactable: {io.Key.gameObject.name}");
                }
            }
           
        }
    }


    /// <summary>
    /// Checks whether any InteractableObject exists within the given radius around the player.
    /// Returns true and all InteractableObjects with distance if found.
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


    public void MouseInteract(){
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (Mouse.current.leftButton.wasPressedThisFrame){
            RayCastFromTouch(mousePosition);
        }
    }

    public void TouchInteract(){
        // TODO
    }

    public void ActivateObject(GameObject obj){
        if (obj.TryGetComponent<InteractableObject>(out InteractableObject interactable)){
            interactable.OnInteract();
        }else if (obj.GetComponentInParent<InteractableObject>() != null){
            Debug.Log($"{DebugID} Parent interactable found");
            obj.GetComponentInParent<InteractableObject>()?.OnInteract();
        }
        else{
            return;
        }
        this.player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    public void RayCastFromTouch(Vector2 touchPos){
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (Physics.Raycast(ray, out RaycastHit hit)){
            if (hit.collider != null){
                ActivateObject(hit.collider.gameObject);
            }
        }
    }


}