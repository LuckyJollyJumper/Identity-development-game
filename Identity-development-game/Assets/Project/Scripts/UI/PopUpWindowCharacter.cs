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

    /// <summary>
    /// Can be removed, was used to test a typing effect for the text
    /// </summary>
    /// <param name="text"></param>
    public override void SetPopUpText(string text){
        this.PopUpText.text = text; 
        // this.TypingEffect.StartEffect();
    }

    /// <summary>
    /// Used by the character to start the quest by setting it as active in the questmanager when the pop-up is closed, so the quest can be tracked and completed.
    /// </summary>
    /// <param name="quest"></param>
    public void StartQuestOnClose(string questName){
        OnPopUpClosed += () => QuestManager.Instance.SetQuestAsActive(questName, NPC.gameObject);
    }

    public void SetCharacterPopUpText(List<string> texts, string name){
        base.PopUpTexts = texts;
        this.NameText.text = name;
    }

}
