using UnityEngine;
using System.Collections.Generic;

public class InteractableCharacter : InteractableObject
{
    [Header("Character Info")]
    [SerializeField] public string characterName;
    [Tooltip("List of dialogue texts for the character")]
    [SerializeField] public List<string> dialogueTexts;

    public override void Start(){
        base.UI.GetComponent<PopUpWindowCharacter>().SetCharacterPopUpText(dialogueTexts, characterName);
        base.Start();
    }
    
}
