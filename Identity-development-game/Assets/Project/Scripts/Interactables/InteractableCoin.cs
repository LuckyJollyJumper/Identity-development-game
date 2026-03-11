using UnityEngine;

public class InteractableCoin : InteractableObject
{
    public override void Start(){
        base.UI.GetComponentInChildren<PopUpWindow>().OnPopUpClosed += () =>{
            GameManager.Instance.AddQuestRewards(1, 0);
            Destroy(this.gameObject);
        };
        this.InteractionObject.SetActive(false);
        this.UI.SetActive(false);
        this.DebugID = $"[InteractableObject/{this.gameObject.name}]";
    }
}
