using UnityEngine;

public class UIPersonaGame : MonoBehaviour
{
    [SerializeField] public GameObject grid;
    public void OnSubmitPersona(){
        // TODO:cGet data and save to PlayerData
        //this.grid.GetComponentsInChildren<TMPro.TMP_InputField>();
        GameManager.Instance.LoadScene(ScenesManager.scenes.MainMenu);
    }
}
