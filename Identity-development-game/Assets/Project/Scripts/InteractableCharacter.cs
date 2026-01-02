using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    [SerializeField] public TextBubble textBubble;

    [Header("Character Info")]
    [SerializeField] public string name;
    [SerializeField] public string popUpText;
    [SerializeField] public string dialogueText;

    public override void Start(){
        textBubble.SetBubbleText(popUpText);
       
    }
    public override void OnEndReadyForInteraction(){
        base.OnEndReadyForInteraction();
    }
}
