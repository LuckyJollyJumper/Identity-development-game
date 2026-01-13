using UnityEngine;
using UnityEngine.UI;

public class PopUpWindowCharacter: PopUpWindow
{
    [Header("Character Chat References")]
    [SerializeField] public InteractableCharacter NPC;
    [SerializeField] public TMPro.TextMeshProUGUI nameText;
    
    public override void Start(){
        base.player    = FindFirstObjectByType<Playercontroller>();
        base.popUpText = GameObject.Find("PopUpText").GetComponent<TMPro.TextMeshProUGUI>();
        this.nameText  = GameObject.Find("NameText").GetComponent<TMPro.TextMeshProUGUI>();

        this.nameText.text = NPC.characterName;
        base.popUpTexts    = NPC.dialogueTexts;
        base.player.StartInteraction();

        NextPopUpText();
    }

    public void CloseChat(){
        EndInteraction();
    }

}
