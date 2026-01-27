using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Child of SelectedItem that includes an Input textfield instead of the image
/// </summary>
public class SelectedItemText : SelectedItem
{
    private TMPro.TMP_InputField InputField;
    public override void Awake(){
        this.Background = this.transform.Find("BackgroundImage").GetComponent<UnityEngine.UI.Image>();

        this.InputField = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
        this.InputField.interactable = false;
    }

    public override void OnPointerClick(PointerEventData eventData){
        base.OnPointerClick(eventData);
        this.InputField.interactable = base.IsSelected;
    }

    /// <summary>
    /// Used by the InputField update the displayText with every change for saving later
    /// </summary>
    public void OnTextChanged(string newText){
        this.DisplayText = this.InputField.text;
        Debug.Log("Updated display text to: " + this.DisplayText);
    }
    
}
