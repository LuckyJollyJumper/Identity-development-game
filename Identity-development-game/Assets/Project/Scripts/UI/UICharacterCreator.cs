using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI class that controls all the UI elements for the character creator
/// </summary>
public class UICharacterCreator : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField InputField; // Used for the nameSelectorPanel
    private GameObject NamePanel;
    private GameObject StylePanel;

    public void Start(){
        this.NamePanel = this.transform.Find("NameSelectorPanel").gameObject;
        this.StylePanel = this.transform.Find("StylePanel").gameObject;
        // this.InputField = GetComponent<TMPro.TMP_InputField>();// Only works because there is only one instance

        this.NamePanel.SetActive(true);
        this.StylePanel.SetActive(false);
    }

    public void OnNameSubmit(){
        string text = this.InputField.text;
        this.InputField.interactable = false;
        this.NamePanel.SetActive(false);

        GameManager.Instance.SetPlayerData("PlayerName", text);

        this.StylePanel.GetComponentInChildren<TMPro.TMP_Text>().text = "Welkom " + text;
        this.StylePanel.SetActive(true);
    }

    public void OnStyleSubmit(){
         // TODO: Save style selection to PlayerData
        GameManager.Instance.LoadScene(ScenesManager.Scenes.PersonaGame);
    }
}
