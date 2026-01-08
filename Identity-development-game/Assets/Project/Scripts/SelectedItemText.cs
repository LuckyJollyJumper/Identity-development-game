using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemText : SelectedItem
{
    private TMPro.TMP_InputField inputField;
    public override void Awake(){
        base.Awake();
        this.inputField = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
        this.inputField.interactable = false;
    }

    public override void OnPointerClick(PointerEventData eventData){
        base.OnPointerClick(eventData);
        this.inputField.interactable = isSelected;
    }

    public void onTextChanged(string newText){
        this.displayText = this.inputField.text;
        Debug.Log("Updated display text to: " + this.displayText);
    }
    
}
