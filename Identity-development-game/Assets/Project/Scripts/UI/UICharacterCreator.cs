using UnityEngine;
using UnityEngine.UI;

public class SubmitFromButton : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField inputField;
    [SerializeField] public GameObject namePanel;
    [SerializeField] public GameObject stylePanel;

    public void Start(){
        if (inputField == null){
            inputField = GetComponent<TMPro.TMP_InputField>();
        }
        namePanel.SetActive(true);
        stylePanel.SetActive(false);
    }
    public void OnNameSubmit(){
        string text = inputField.text;
        inputField.interactable = false;
        namePanel.SetActive(false);

        GameManager.Instance.SetPlayerDataField("playerName", text);

        stylePanel.GetComponentInChildren<TMPro.TMP_Text>().text = "Welkom " + text;
        stylePanel.SetActive(true);
    }

    public void OnStyleSubmit(){
         // TODO: Save style selection to PlayerData
        stylePanel.SetActive(false);
        GameManager.Instance.LoadScene(ScenesManager.scenes.PersonaGame);
        GameManager.Instance.SaveGame();
       
    }
}
