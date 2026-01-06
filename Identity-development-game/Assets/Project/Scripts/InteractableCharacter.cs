using UnityEngine;

public class InteractableCharacter : InteractableObject
{

    [Header("Character Info")]
    [SerializeField] public string name;
    [SerializeField] public string popUpText;
    [SerializeField] public string dialogueText;

    public override void Start(){
        this.interactionObject.GetComponent<TextBubble>().SetBubbleText(popUpText);

        this.UI.SetActive(false);
        this.interactionObject.SetActive(false);
    }
    
}
