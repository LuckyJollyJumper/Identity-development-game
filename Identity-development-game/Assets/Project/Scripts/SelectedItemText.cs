using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemText : SelectedItem
{
    private TMPro.TMP_InputField inputField;
    public override void Awake()
    {
        this.inputField = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
        this.inputField.interactable = false;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        this.inputField.interactable = isSelected;
        this.GetComponent<UnityEngine.UI.RawImage>().color = isSelected ? Color.green : Color.white;
    }

    public void onTextChanged(string newText)
    {
        this.category = this.inputField.text;
        Debug.Log("Updated category to: " + this.category);
    }
    
}
