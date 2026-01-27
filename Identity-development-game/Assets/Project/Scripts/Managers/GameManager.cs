using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Game manager to handle Startup, Saving/Loading, and overall game state.
/// </summary>     
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance{ // Make sure only one instance of GameManager exists
        get{
            if (instance == null){
                instance = FindFirstObjectByType<GameManager>();

                if (instance == null){
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }

    [SerializeField] public ScenesManager _scenesManager;
    [HideInInspector] public PlayerData _playerData;
    [HideInInspector] public ServerData _serverData;
    private JsonSaveSystem _jsonSaveSystem;
    public enum GameProgressStage{
       FirstVisitIntroduction,
       InteractedWithLeaderBoard,
       InteractedWithFirstNPC,
       CompletedFirstActivity,

    }
    private string DebugID = "[GameManager]";
   
    public void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this){ Destroy(this.gameObject); }

        PrepareGameData();

        _scenesManager = GetComponentInChildren<ScenesManager>();
    }

    public void PrepareGameData(){
        Debug.Log($"{DebugID} Game Started");
        _jsonSaveSystem = new JsonSaveSystem();
    
        // Load player data if it is on disk otherwise start new character creation
        (bool playerPresent, PlayerData data) = _jsonSaveSystem.LoadPlayerData();
        _playerData = data;
        if (playerPresent){
            _scenesManager.LoadScene(ScenesManager.Scenes.CharacterCreator);
        }
        else{
            UIPlayerInfo playerHud = FindFirstObjectByType<UIPlayerInfo>();
            playerHud.DisplayPlayerData(_playerData);
        }
        
        // Load server data for access during play
        _serverData = _jsonSaveSystem.LoadServerData();
    }


    //--------------------------------------------------//
    // Functions to be called by UI or other managers
    //--------------------------------------------------//
    public void SaveGame(){ _jsonSaveSystem.SavePlayerData(_playerData); }
    public void QuitGame(){ _scenesManager.QuitGame(); }
    public void LoadScene(ScenesManager.Scenes scene){ _scenesManager.LoadScene(scene); }
    public void DeleteSave(){ 
        _jsonSaveSystem.DeleteSaveData();
        _playerData = new PlayerData();
        _scenesManager.LoadScene(ScenesManager.Scenes.CharacterCreator);
    }



   /// <summary>
   /// Sets a field on the PlayerData instance by name, parsing the raw string value as needed.
   /// Supports string, int, and string[] types.
   /// </summary>
    public bool SetPlayerDataField(string fieldName, string rawValue){
        _playerData ??= new PlayerData(); //If PlayerData is null, create a new one

        var targetField = typeof(PlayerData).GetField(fieldName);
        if (targetField == null){
            Debug.LogWarning($"{DebugID} SetPlayerDataField: field '{fieldName}' not found on PlayerData.");
            return false;
        }

        Type targetType = targetField.FieldType;
        try{
            if (targetType == typeof(string)){
                targetField.SetValue(_playerData, rawValue);
                return true;
            }

            if (targetType == typeof(int)){
                if (int.TryParse(rawValue, out int iv)){
                    targetField.SetValue(_playerData, iv);
                    return true;
                }
                Debug.LogWarning($"{DebugID} SetPlayerDataField: cannot parse '{rawValue}' as int for field '{fieldName}'.");
                return false;
            }

            if (targetType == typeof(string[])){
                string[] parts;
                if (string.IsNullOrEmpty(rawValue)){
                    parts = new string[] { };
                }
                else if (rawValue.StartsWith("[") && rawValue.EndsWith("]")){
                    // Use JsonUtility to parse proper JSON array format
                    try{
                        // Wrap in a temporary container class for JsonUtility parsing
                        string jsonWrapped = "{\"items\":" + rawValue + "}";
                        StringArrayWrapper wrapper = JsonUtility.FromJson<StringArrayWrapper>(jsonWrapped);
                        parts = wrapper.items ?? new string[] { };
                    }
                    catch (Exception parseEx){
                        Debug.LogWarning($"{DebugID} SetPlayerDataField: JSON array parsing failed for '{rawValue}': {parseEx.Message}. Attempting fallback comma-split.");
                        // Fallback: simple comma-separated parsing
                        string inner = rawValue.Substring(1, rawValue.Length - 2).Trim();
                        parts = string.IsNullOrEmpty(inner) 
                            ? new string[] { } 
                            : inner.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s => s.Trim().Trim('"')).ToArray();
                    }
                }
                else{
                    // Treat as comma-separated list
                    parts = rawValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => s.Trim()).ToArray();
                }

                targetField.SetValue(_playerData, parts);
                return true;
            }

            // fallback for other primitive types
            object converted = Convert.ChangeType(rawValue, targetType);
            targetField.SetValue(_playerData, converted);
            return true;
        }
        catch (Exception ex){
            Debug.LogWarning($"{DebugID} SetPlayerDataField: failed to set '{fieldName}' with value '{rawValue}': {ex.Message}");
            return false;
        }

    }
    

}

/// <summary>
/// Temporary wrapper class for JsonUtility to deserialize string arrays.
/// Used internally by GameManager.SetPlayerDataField.
/// </summary>
[System.Serializable]
public class StringArrayWrapper{
    public string[] items;
}
