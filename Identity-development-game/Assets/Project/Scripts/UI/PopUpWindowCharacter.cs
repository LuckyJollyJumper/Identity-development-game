using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// PopUpWindow child that adds a nametext box to the PopUp
/// </summary>
public class PopUpWindowCharacter: PopUpWindow
{
    [Header("Character Chat References")]
    [Tooltip("Reference to parent object")]
    [SerializeField] public InteractableCharacter NPC;
    [SerializeField] TMPro.TextMeshProUGUI NameText;
    [HideInInspector] TypingEffect TypingEffect;
    
    public override void Start(){
        base.Start();
         foreach (TMPro.TextMeshProUGUI t in GetComponentsInChildren<TMPro.TextMeshProUGUI>()){
            if (t.name == "NameText"){
                this.NameText = t.GetComponent<TMPro.TextMeshProUGUI>();
                // this.TypingEffect = t.GetComponent<TypingEffect>();
            }
        }
    }

    public override void SetPopUpText(string text){
        this.PopUpText.text = text; 
        // this.TypingEffect.StartEffect();
    }

    public void SetCharacterPopUpText(List<string> texts, string name){
        base.PopUpTexts = texts;
        this.NameText.text = name;
    }

}
