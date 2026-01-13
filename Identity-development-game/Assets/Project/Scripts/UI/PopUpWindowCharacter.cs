using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopUpWindowCharacter: PopUpWindow
{
    [Header("Character Chat References")]
    [SerializeField] public InteractableCharacter NPC;
    [SerializeField] public TMPro.TextMeshProUGUI nameText;
    
    public override void Start(){
        this.nameText  = GameObject.Find("NameText").GetComponent<TMPro.TextMeshProUGUI>();
        base.Start();
    }

    public void SetCharacterPopUpText(List<string> texts, string name){
        base.popUpTexts = texts;
        nameText.text = name;
    }

}
