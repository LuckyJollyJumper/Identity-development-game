using UnityEngine;

/// <summary>
/// The superclass of all interactables in the game. They all have a method for when the player walks into
/// proximity (Shows ) and when the player interacts with the object.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    public enum ObjectState { Idle, ReadyForInteraction, Interacting };

    [Header("References")]
    [Tooltip("Object that gets activated when player is in proximity")]
    [SerializeField] public GameObject InteractionObject;
    [Tooltip("UI that appears when player interacts with the object")]
    [SerializeField] public GameObject UI;
    [SerializeField] public ObjectState CurrentState = ObjectState.Idle;
    [Header("Interaction")]
    [SerializeField] public string PopUpText;
    [Tooltip("Sound that plays when the player interacts with the object, can be empty")]
    [SerializeField] private AudioClip InteractionSound;
    
    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    private Playercontroller InteractingPlayer;
    [HideInInspector] public string DebugID = "[InteractableObject]";

    public virtual void Start(){
        this.InteractionObject.GetComponentInChildren<TextBubble>().SetBubbleText(PopUpText);
        this.InteractionObject.SetActive(false);
        this.UI.SetActive(false);
        this.DebugID = $"[InteractableObject/{this.gameObject.name}]";
    }

    /// <summary>
    /// Gets called when the player walks into proximity of this object.
    /// Default: turns the interactionObjects visible.
    /// </summary>
    public virtual void OnReadyForInteraction(){
        if (this.InteractionObject != null){
            this.InteractionObject.SetActive(true);
        }
    }

    /// <summary>
    /// Gets called when the player clicks on the object in the world 
    /// </summary>
    /// <param name="player">Pass player for closing and for interactions that concern the player</param>
    public virtual void OnInteract(Playercontroller player){
        this.UI.SetActive(true);
        this.InteractingPlayer = player;
        PlayInteractionSound();
    }

    /// <summary>
    /// Gets called when the player stops interacting with the object.
    /// Default: closing the UI that is connected to this object.
    /// </summary>
    public virtual void OnEndInteract(){
        this.UI.SetActive(false);
        if (DebugMode){ Debug.Log($"{DebugID} Ending interaction with player"); }
        this.InteractingPlayer.EndInteraction();
    } 

    /// <summary>
    /// Gets called when the player moves out of proximity.
    /// Default: set the objects to invisible again. 
    /// </summary>
    public virtual void OnEndReadyForInteraction(){
        this.InteractionObject.SetActive(false);
    }

    /// <summary>
    /// Plays the interaction sound if it exists.
    /// </summary>
    private void PlayInteractionSound(){
        if (InteractionSound != null){
            if (DebugMode){ Debug.Log($"{DebugID} Playing interaction sound"); }
            SoundManager.Instance.PlaySound(InteractionSound, this.transform);
        }
    }


    // ------------NOT USED AT THE MOMENT------------//
    /// <summary>
    /// State machine for the InteractableObjects
    /// </summary>
    /// <param name="newState"></param>
    public void SetInteractionState(ObjectState newState){
        if (newState == this.CurrentState) return;
        if (newState == ObjectState.ReadyForInteraction){  // Player walks into proximity
            OnReadyForInteraction();
        }else if (newState == ObjectState.Idle){
            if (this.CurrentState == ObjectState.ReadyForInteraction){ // Player walks out of proximity
                OnEndReadyForInteraction();
            }
            else{ OnEndInteract(); } // Player ends interaction
        }

        this.CurrentState = newState;
        if (DebugMode){ Debug.Log($"{DebugID} {gameObject.name} changed state to {this.CurrentState}"); }
    }
}
