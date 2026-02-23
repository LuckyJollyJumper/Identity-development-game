using UnityEngine;

public class InteractableHiddenObject: InteractableObject
{
    public override void Start(){
        base.Start();
        this.InteractionObject.GetComponentInChildren<TextBubble>().SetBubbleText(PopUpText, 20);
    }

    public void InteractedWHiddenObject(){
        Debug.Log("Found object");
    }
}
