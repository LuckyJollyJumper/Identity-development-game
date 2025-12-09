using UnityEngine;

public class UIPersonaGame : MonoBehaviour
{
    public void OnSubmitPersona(){
        // TODO:cGet data and save to PlayerData
        GameManager.Instance.LoadScene(ScenesManager.scenes.MainMenu);
    }
}
