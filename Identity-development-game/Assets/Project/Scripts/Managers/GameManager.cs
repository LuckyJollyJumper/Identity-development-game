using UnityEngine;
//using PL = Assets.Project.Scripts.Player.Player;

/// <summary>
/// Game manager to handle Startup, Saving/Loading, and overall game state.
/// </summary>     
public class GameManager : MonoBehaviour
{
    [SerializeField] public ScenesManager _scenesManager;
    [HideInInspector] public PlayerData _playerData;
    private JsonSaveSystem _jsonSaveSystem;
   
    public void Awake(){
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("===== Game started =====");
        _jsonSaveSystem = new JsonSaveSystem();

        (bool newPlayer, PlayerData data) = _jsonSaveSystem.LoadPlayerData();
        if (newPlayer){
            _scenesManager.LoadScene(ScenesManager.scenes.CharacterCreator);
        }
        else{
            _playerData = data;
            UIMainMenu uiMainMenu = FindFirstObjectByType<UIMainMenu>();
            uiMainMenu.LoadPlayerData(data);
        }

    }

    public void SaveGame(){
        // Grab all data
        _jsonSaveSystem.SavePlayerData(_playerData);
    }

    public void DeleteSave(){
        _jsonSaveSystem.DeleteSaveData();
    }

    public void QuitGame(){
        _scenesManager.QuitGame();
    }

    public void DisplayPlayerData(){
        
    }

}
