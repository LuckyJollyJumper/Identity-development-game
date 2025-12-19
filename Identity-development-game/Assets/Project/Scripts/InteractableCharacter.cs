using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public override void OnEndReadyForInteraction(){
        base.OnEndReadyForInteraction();
        Debug.Log("Character ended ready for interaction: " + this.gameObject.name);
    }
}
