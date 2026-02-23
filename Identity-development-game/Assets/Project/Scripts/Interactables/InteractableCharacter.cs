using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// InteractableObject for NPC characters. Uses a different PopUpWindow than base.
/// </summary>
public class InteractableCharacter : InteractableObject
{
    [Header("Character Info")]
    [SerializeField] public string CharacterName;
    [Tooltip("List of dialogue texts for the character")]
    [TextArea][SerializeField] public List<string> DialogueTexts;
    [SerializeField] string NPCQuestName; // The quest that the NPC gives, used to signal the questmanager

    public override void Start(){
        base.UI.GetComponent<PopUpWindowCharacter>().SetCharacterPopUpText(DialogueTexts, CharacterName);
        if (NPCQuestName != ""){// Ignore sending the quest if the NPC has none
            base.UI.GetComponent<PopUpWindowCharacter>().StartQuestOnClose(NPCQuestName);
        }
        base.Start();
    }
    
}
