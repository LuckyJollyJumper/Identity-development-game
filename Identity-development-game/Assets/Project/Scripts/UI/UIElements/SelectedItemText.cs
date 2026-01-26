using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemText : SelectedItem
{
    private TMPro.TMP_InputField InputField;
    public override void Awake(){
        base.Awake();
        this.InputField = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
        this.InputField.interactable = false;
    }

    public override void OnPointerClick(PointerEventData eventData){
        base.OnPointerClick(eventData);
        this.InputField.interactable = base.IsSelected;
    }

    public void onTextChanged(string newText){
        this.DisplayText = this.InputField.text;
        Debug.Log("Updated display text to: " + this.DisplayText);
    }
    
}
