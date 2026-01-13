using UnityEngine;
using System.Collections.Generic;

public class InteractableCharacter : InteractableObject
{
    [Header("Character Info")]
    [SerializeField] public string characterName;
    [SerializeField] public string popUpText;
    [Tooltip("List of dialogue texts for the character")]
    [SerializeField] public List<string> dialogueTexts;

    public override void Start(){
        this.interactionObject.GetComponent<TextBubble>().SetBubbleText(popUpText);

        this.UI.SetActive(false);
        this.interactionObject.SetActive(false);
    }
    
}
