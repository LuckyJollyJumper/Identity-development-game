using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public GameObject interactionObject;
    [SerializeField] public GameObject UI;
    public enum ObjectState { Idle, ReadyForInteraction, Interacting };
    public ObjectState currentState = ObjectState.Idle;

    public virtual void OnReadyForInteraction(){
       this.interactionObject.SetActive(true);
    }

    public virtual void OnInteract(){
        this.UI.SetActive(true);
    }

    public virtual void OnEndInteract(){
        this.UI.SetActive(false);
    }

    public void OnEndReadyForInteraction(){
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
    }
}
