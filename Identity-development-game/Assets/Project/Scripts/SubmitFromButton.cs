using UnityEngine;
using UnityEngine.UI;

public class SubmitFromButton : MonoBehaviour
{
    [SerializeField] public InputField inputField;
    void OnSubmit()
    {
        string text = inputField.text;
        Debug.Log("Input Field Text: " + text);
        inputField.interactable = false;
    }
}
