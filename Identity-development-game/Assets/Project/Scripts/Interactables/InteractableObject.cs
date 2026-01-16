using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum ObjectState { Idle, ReadyForInteraction, Interacting };

    [Header("References")]
    [Tooltip("Object that gets activated when player is in proximity")]
    [SerializeField] public GameObject interactionObject;
    [Tooltip("UI that appears when player interacts with the object")]
    [SerializeField] public GameObject UI;
    [SerializeField] public ObjectState currentState = ObjectState.Idle;
    [SerializeField] public string popUpText;
    
    [Header("Debug")]
    [SerializeField] public bool debugMode = false;
    private Playercontroller interactingPlayer;
    private string DebugID = "[InteractableObject]";

    public virtual void Start(){
        this.interactionObject.GetComponentInChildren<TextBubble>().SetBubbleText(popUpText);
        this.interactionObject.SetActive(false);
        this.UI.SetActive(false);
    }

    public virtual void OnReadyForInteraction(){
       this.interactionObject.SetActive(true);
    }

    // Pass player for closing
    public virtual void OnInteract(Playercontroller player){
        this.UI.SetActive(true);
        this.interactingPlayer = player;
    }

    public virtual void OnEndInteract(){
        this.UI.SetActive(false);
        if (debugMode){ Debug.Log($"{DebugID} Ending interaction with player"); }
        this.interactingPlayer.EndInteraction();
    } 

    public virtual void OnEndReadyForInteraction(){
        this.interactionObject.SetActive(false);
    }

    public void SetInteractionState(ObjectState newState){
        if (newState == this.currentState) return;
        if (newState == ObjectState.ReadyForInteraction){  // Player walks into proximity
            OnReadyForInteraction();
        }else if (newState == ObjectState.Idle){
            if (this.currentState == ObjectState.ReadyForInteraction){ // Player walks out of proximity
                OnEndReadyForInteraction();
            }
            else{ OnEndInteract(); } // Player ends interaction
        }

        this.currentState = newState;
        if (debugMode){ Debug.Log($"{DebugID} {gameObject.name} changed state to {this.currentState}"); }
    }
}
