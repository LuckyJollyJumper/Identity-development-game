using UnityEngine;

/// <summary>
/// InteractableObject child that only uses the proximity of the player and has no interaction
/// </summary>
public class InteractableDoor : InteractableObject
{
    [SerializeField] public Animator SlidingDoorAnimator;

    public override void Start(){ }
    public override void OnReadyForInteraction(){
        SlidingDoorAnimator.SetTrigger("SlidingDoorOpen");
    }

    public override void OnEndReadyForInteraction(){
        SlidingDoorAnimator.SetTrigger("SlidingDoorClose");
    }

    public override void OnInteract(Playercontroller player){
        player.EndInteraction();
    }
    public override void OnEndInteract(){ }


}
