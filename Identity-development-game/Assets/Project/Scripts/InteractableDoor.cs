using UnityEngine;

public class InteractableDoor : InteractableObject
{
    [SerializeField] public Animator SlidingDoorAnimator;

    public override void Start(){ }
    public override void OnReadyForInteraction(){
        Debug.Log("Door opening");
        SlidingDoorAnimator.SetTrigger("SlidingDoorOpen");
    }

    public override void OnEndReadyForInteraction(){
        Debug.Log("Door closing");
        SlidingDoorAnimator.SetTrigger("SlidingDoorClose");
    }

    public override void OnInteract(Playercontroller player){ 
        Debug.Log("Door interacted with");
        player.EndInteraction();
    }
    public override void OnEndInteract(){ }


}
