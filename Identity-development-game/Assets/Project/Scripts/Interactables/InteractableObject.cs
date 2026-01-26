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
    [SerializeField] public string PopUpText;
    
    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    private Playercontroller InteractingPlayer;
    private string DebugID = "[InteractableObject]";

    public virtual void Start(){
        this.InteractionObject.GetComponentInChildren<TextBubble>().SetBubbleText(PopUpText);
        this.InteractionObject.SetActive(false);
        this.UI.SetActive(false);
    }

    public virtual void OnReadyForInteraction(){
       this.InteractionObject.SetActive(true);
    }

    // Pass player for closing and for interactions that concern the player
    public virtual void OnInteract(Playercontroller player){
        this.UI.SetActive(true);
        this.InteractingPlayer = player;
    }

    public virtual void OnEndInteract(){
        this.UI.SetActive(false);
        if (DebugMode){ Debug.Log($"{DebugID} Ending interaction with player"); }
        this.InteractingPlayer.EndInteraction();
    } 

    public virtual void OnEndReadyForInteraction(){
        this.InteractionObject.SetActive(false);
    }

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
