using UnityEngine;
using Terresquall;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    [SerializeField] public GameObject JoystickObject;
    
    private Rigidbody body;
    public enum InteractionState{Moving, Interacting}; // Moving = Can move and interact, Interacting = Only interacting with UI
    private InteractionState playerState;

    [Header("Debug")]
    [SerializeField] public bool DebugMode = true;
    private string DebugID = "[PlayerController]";

    // Footstep sound support
    [Header("Audio")]
    [Tooltip("Minimum time in seconds between consecutive footstep sounds while moving")]
    [SerializeField] private float footstepDelay = 0.5f;
    private float footstepTimer = 0f;

    void Start(){
        if (player != null) this.body = player.GetComponent<Rigidbody>();
        this.playerState = InteractionState.Moving;

        // ensure enhanced touch is enabled so we can read Touch.activeTouches
        EnhancedTouchSupport.Enable();
    }

    void Update(){
        // keep non-physics checks and input polling here
        if (playerState == InteractionState.Moving){
            // Check for object in proximity and activate them
            ActivateSurroundingObjects();
        }

        // switch between input methods depending on environment
        bool useTouch = Application.isMobilePlatform ||
                        (Touchscreen.current != null && Touchscreen.current.enabled);
        if (useTouch){
            TouchInteract();
        } else {
            MouseInteract();
        }
    }

    void OnDisable(){
        EnhancedTouchSupport.Disable();
    }

    void FixedUpdate(){
        if (playerState != InteractionState.Moving) return;

        MovePlayer();
        HandleFootsteps();
    }

    public void MovePlayer(){
        if (!VirtualJoystick.instances.ContainsKey(JID)) return;

        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);

        Vector3 input = new Vector3(h, 0.05f, v);
        if (player == null) return;
        if (body == null) {body = player.GetComponent<Rigidbody>();}

        // Apply movement via Rigidbody velocity for deterministic physics
        if (body != null){
            Vector3 currentVel = body.linearVelocity;
            Vector3 desiredVel = input.normalized * speed;
            // preserve vertical velocity (gravity/jumps)
            desiredVel.y = currentVel.y;

            // If there's negligible input, stop horizontal movement
            if (input.magnitude <= 0.01f){
                desiredVel.x = 0f;
                desiredVel.z = 0f;
            }

            body.linearVelocity = desiredVel;

            // Rotate to face movement direction using MoveRotation
            Vector3 direction = new Vector3(h, 0f, v);
            if (direction.magnitude > 0.1f){
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion newRot = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
                body.MoveRotation(newRot);
            }
        }else{
            // If no Rigidbody, fallback to transform movement (legacy)
            Vector3 delta = input * speed * Time.deltaTime;
            player.transform.position += delta;
            Vector3 direction = input;
            if (direction.magnitude > 0.1f){
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    /// <summary>
    /// Plays footstep sounds at intervals when the player is moving.
    /// A timer is used to prevent the sound from being played every physics frame.
    /// </summary>
    private void HandleFootsteps(){
        // only play when there's substantial horizontal movement
        Vector3 horizontalVel = Vector3.zero;
        if (body != null)
            horizontalVel = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
        
        if (horizontalVel.magnitude > 0.1f){
            footstepTimer += Time.fixedDeltaTime;
            if (footstepTimer >= footstepDelay){
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayFootstepSound();
                footstepTimer = 0f;
            }
        }
        else{
            // reset timer when not moving so sound plays immediately after next movement
            footstepTimer = 0f;
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
                if (io.Value <= interactionRadius && io.Key.CurrentState == InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.ReadyForInteraction);
                }else if (io.Value > interactionRadius && io.Key.CurrentState != InteractableObject.ObjectState.Idle){
                    io.Key.SetInteractionState(InteractableObject.ObjectState.Idle);
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
            // Check if pointer is over UI elements - if so, don't process interaction
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1)){
                if (DebugMode){ Debug.Log($"{DebugID} Mouse click over UI - interaction blocked"); }
                return;
            }
            RayCastFromTouch(mousePosition);
        }
    }

    public void TouchInteract(){
        // use EnhancedTouch to support multiple fingers / touches
        if (Touchscreen.current == null) return;

        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches){
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began){
                // Check if pointer is over UI elements - if so, don't process interaction
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.touchId)){
                    if (DebugMode){ Debug.Log($"{DebugID} Touch over UI (ID: {touch.touchId}) - interaction blocked"); }
                    continue;
                }
                RayCastFromTouch(touch.screenPosition);
            }
        }
    }

    public void RayCastFromTouch(Vector2 touchPos){
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        int uiWindowLayer = LayerMask.NameToLayer("UIWindow");
        if (Physics.Raycast(ray, out RaycastHit hit)){
            if (hit.collider != null){
                // Check if UIWindow layer is blocking interaction with 3D objects below
                if (hit.collider.gameObject.layer == uiWindowLayer){
                    if (DebugMode){ Debug.Log($"{DebugID} We hit a UIWindow"); }
                    // UIWindow blocks further interaction - don't activate 3D objects beneath it
                    return;
                }
                ActivateObject(hit.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// Activates the given GameObject to interacting if it has an InteractableObject component (or in parent) 
    /// and sets the player state to Interacting.
    /// </summary>
    public void ActivateObject(GameObject obj){
        if (obj.TryGetComponent<InteractableObject>(out InteractableObject interactable)){
            SetPlayerState(InteractionState.Interacting);
            interactable.OnInteract(this);
        }else if (obj.GetComponentInParent<InteractableObject>() != null){
            SetPlayerState(InteractionState.Interacting);
            obj.GetComponentInParent<InteractableObject>()?.OnInteract(this);
        }
        else{
            return;
        }
    }

    /// <summary>
    /// Ends interaction with the current InteractableObject and sets player state back to Moving. 
    /// Should be called from InteractableObjects when ending interaction.
    /// </summary>
    public void EndInteraction(){
        JoystickObject.SetActive(true); 
        SetPlayerState(InteractionState.Moving);
    }

    /// <summary>
    /// Starts interaction with an InteractableObject and sets player state to Interacting. Should be called 
    /// from InteractableObjects when starting interaction.
    /// </summary>
    public void StartInteraction(){
        JoystickObject.SetActive(false); 
        if (body == null && player != null) body = player.GetComponent<Rigidbody>();
        if (body != null) body.linearVelocity = Vector3.zero;
        SetPlayerState(InteractionState.Interacting);
    }

    /// <summary>
    /// State machine for the player interactions. Should be set to Interacting when activating an 
    /// InteractableObject and set back to Moving when ending interaction.
    /// </summary>
    /// <param name="newState"></param>
    public void SetPlayerState(InteractionState newState){
        // Assumes only 2 states for now
        if (newState == this.playerState) return;
        if (newState == InteractionState.Interacting){
            if (DebugMode){ Debug.Log($"{DebugID} Setting state to Interacting"); }
        }
        else if (newState == InteractionState.Moving){ 
            if (DebugMode){ Debug.Log($"{DebugID} Setting state to Moving"); }
        }
        this.playerState = newState;
    }

}