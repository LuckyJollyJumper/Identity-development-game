using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI class that controls all the UI elements for the character creator
/// </summary>
public class SubmitFromButton : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField InputField;
    [SerializeField] public GameObject NamePanel;
    [SerializeField] public GameObject StylePanel;

    public void Start(){
        if (this.InputField == null){
            this.InputField = GetComponent<TMPro.TMP_InputField>();
        }
        this.NamePanel.SetActive(true);
        this.StylePanel.SetActive(false);
    }
    public void OnNameSubmit(){
        string text = this.InputField.text;
        this.InputField.interactable = false;
        this.NamePanel.SetActive(false);

        GameManager.Instance.SetPlayerDataField("playerName", text);

        this.StylePanel.GetComponentInChildren<TMPro.TMP_Text>().text = "Welkom " + text;
        this.StylePanel.SetActive(true);
    }

    public void OnStyleSubmit(){
         // TODO: Save style selection to PlayerData
        this.StylePanel.SetActive(false);
        GameManager.Instance.LoadScene(ScenesManager.Scenes.PersonaGame);
        GameManager.Instance.SaveGame();
       
    }
}
