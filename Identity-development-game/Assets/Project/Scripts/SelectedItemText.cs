using UnityEngine;

public class SelectedItemText : SelectedItem
{
    private TMPro.TMP_InputField inputField;
    public override void Awake()
    {
        this.inputField = gameObject.GetComponentInChildren<TMPro.TMP_InputField>();
    }
    public void onTextChanged(string newText)
    {
        this.category = this.inputField.text;
        Debug.Log("Updated category to: " + this.category);
    }
    
}
