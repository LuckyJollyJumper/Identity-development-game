using UnityEngine;

public class UIChat : MonoBehaviour
{
    [SerializeField] public InteractableCharacter characterScript;
    [SerializeField] public TMPro.TextMeshProUGUI nameText;
    [SerializeField] public TMPro.TextMeshProUGUI chatText;
    
    void Start(){
        this.nameText.text = characterScript.name;
        this.chatText.text = characterScript.dialogueText;
    }

    public void CloseChat(){
        characterScript.OnEndInteract();
    }

}
