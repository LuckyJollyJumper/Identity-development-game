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
    
    public override void Start(){
        base.UI.GetComponent<PopUpWindowCharacter>().SetCharacterPopUpText(DialogueTexts, CharacterName);
        base.UI.GetComponent<PopUpWindowCharacter>().DeleteOnClose = false; // keep to interact with it more than once
        base.Start();
    }
    
}
