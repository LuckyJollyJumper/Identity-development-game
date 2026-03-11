using UnityEngine;

public class UIStartScreen : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI Text;

    public void Start(){
        Text.text = Application.persistentDataPath;
    }

    /// <summary>
    /// Loads either the character creator or the school map depending on whether player data is already present. 
    /// Called from the start button on the start screen.
    /// </summary>
    public void StartGame(){
        GameManager.Instance.LoadPlayerData();
    }

    public void ResetPlayer(){
        GameManager.Instance.DeleteSave();
    }
}
