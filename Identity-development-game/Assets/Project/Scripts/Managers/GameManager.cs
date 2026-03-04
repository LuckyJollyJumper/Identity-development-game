using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Game manager to handle Startup, Saving/Loading, and overall game state.
/// It has access to the serverdata and playerdata and all other managers.
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

    [Header("Managers")]
    [SerializeField] public ScenesManager _scenesManager;
    // Public singleton data
    [HideInInspector] public PlayerData _playerData;
    [HideInInspector] public readonly float MaxPlayerLevel = 20f;
    [HideInInspector] public ServerData _serverData;
    private JsonSaveSystem _jsonSaveSystem;
    private UISchoolMap _SchoolMapCanvas;
    
    private string DebugID = "[GameManager]";
   
    public void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this){ Destroy(this.gameObject); }

        PrepareGameData();
        Debug.Log($"{DebugID} Game Started");
    }

    /// <summary>
    /// Prepares the game data by initializing the JsonSaveSystem, loading player data from disk 
    /// (or starting character creation if no data is found), and loading server data. Also gets 
    /// references to ScenesManager and UISchoolMap for later use by other gameObjects.
    /// </summary>
    public void PrepareGameData(){
        _jsonSaveSystem = new JsonSaveSystem();
        _scenesManager = GetComponentInChildren<ScenesManager>();
        _SchoolMapCanvas = FindFirstObjectByType<UISchoolMap>();
    
        // Load player data if it is on disk otherwise start new character creation
        (bool playerPresent, PlayerData data) = _jsonSaveSystem.LoadPlayerData();
        _playerData = data;
        if (!playerPresent){
            Debug.Log($"{DebugID} No player found on disk, creating new one");
            SetPlayerID(); // Assign a unique playerID from the server
            _scenesManager.LoadScene(ScenesManager.Scenes.CharacterCreator);
        }
        else{
            UpdatePlayerHUD();
        }
        
        // Load server data for access during play
        SaveFakePlayerData(); // CAN BE REMOVED, only used for testing without server
        _serverData = _jsonSaveSystem.LoadServerData();
    }

    /// <summary>
    /// Only used for testing without a server. Will load in fake players into the local server 
    /// file if they are not there yet
    /// </summary>
    private void SaveFakePlayerData(){
        PlayerData p = new(){
            PlayerID = 0,
            PlayerName = "Carla",
            Points = 3024,
        };
        if (!_serverData.Players.Any(l => l.PlayerID == p.PlayerID && l.PlayerName == p.PlayerName)){ _serverData.Players.Add(p); }
        PlayerData p1 = new(){
            PlayerID = 1,
            PlayerName = "Yusuf",
            Points = 512,
        };
        if (!_serverData.Players.Any(l => l.PlayerID == p1.PlayerID && l.PlayerName == p1.PlayerName)){ _serverData.Players.Add(p1); }
        PlayerData p2 = new(){
            PlayerID = 2,
            PlayerName = "Anna",
            Points = 200,
        };
        if (!_serverData.Players.Any(l => l.PlayerID == p2.PlayerID && l.PlayerName == p2.PlayerName)){ _serverData.Players.Add(p2); }
        PlayerData p3 = new(){
            PlayerID = 3,
            PlayerName = "Max",
            Points = 2103,
        };
        if (!_serverData.Players.Any(l => l.PlayerID == p3.PlayerID && l.PlayerName == p3.PlayerName)){ _serverData.Players.Add(p3); }
        PlayerData p4 = new(){
            PlayerID = 4,
            PlayerName = "Leila",
            Points = 2103,
        };
        if (!_serverData.Players.Any(l => l.PlayerID == p4.PlayerID && l.PlayerName == p4.PlayerName)){ _serverData.Players.Add(p4); }
        if (DebugID){ Debug.Log($"{DebugID} Fake player data saved to server"); }
    }


    //--------------------------------------------------//
    // Functions to be called by UI or other managers
    //--------------------------------------------------//
    public void SaveGame(){ _jsonSaveSystem.SavePlayerData(_playerData); }
    public void QuitGame(){ _scenesManager.QuitGame(); }
    public void LoadScene(ScenesManager.Scenes scene){ _scenesManager.LoadScene(scene); }
    public void LoadSchoolMap(){  _scenesManager.LoadScene(ScenesManager.Scenes.SchoolMap); }
    public void DeleteSave(){ 
        _jsonSaveSystem.DeletePlayerSaveData();
        _playerData = new();
        _scenesManager.LoadScene(ScenesManager.Scenes.CharacterCreator);
    }
    public void UpdatePlayerHUD(){
        _SchoolMapCanvas.GetComponent<UISchoolMap>().UpdatePlayerHUD(_playerData);
    }

    public ServerData GetServerData(){
        _jsonSaveSystem.LoadServerData();
        return this._serverData;
    }




    //-----------------------------------------------------------------//
    // Functions to be called by UI or other managers to change player
    //-----------------------------------------------------------------//
    public void SetPlayerID(){
        GetServerData(); // Make sure we have the latest server data to assign a unique playerID
        int id = this._serverData.NextPlayerID;
        this._playerData.PlayerID = id;
        this._serverData.NextPlayerID += 1;
        _jsonSaveSystem.SaveServerData(this._serverData);
        SaveGame();
    }

    public void LevelUpPlayer(){
        _playerData.LevelUp();
        SaveGame();
    }

    public void AddActivityData(ActivityData data){
        this._playerData.AddActivityData(data);
        SaveGame();
    }

    public void AddQuestData(QuestData data){
        this._playerData.AddQuestData(data);
        SaveGame();
    }

    public void AddQuestRewards(int coins, int points){
        this._playerData.Coins += coins;
        this._playerData.Points += points;
        UpdatePlayerHUD();
        SaveGame();
        
    }

    public void AddInventoryItem(ItemData item){
        this._playerData.AddInventoryItem(item);
        UpdatePlayerHUD();
        SaveGame();
    }

    /// <summary>
    /// Used as the public function to set a value in the PlayerData and automatically saves the game
    /// to reduce function calls with GameManager Instance
    /// </summary>
    public void SetPlayerData(string fieldName, string rawValue){
        SetPlayerDataField(fieldName, rawValue);
        SaveGame();
    }
   /// <summary>
   /// Sets a field on the PlayerData instance by name, parsing the raw string value as needed.
   /// Supports string, int, string[], and List<T> types.
   /// </summary>
    private bool SetPlayerDataField(string fieldName, string rawValue){
        _playerData ??= new(); //If PlayerData is null, create a new one

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

            // Handle List<T> types
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)){
                Type elementType = targetType.GetGenericArguments()[0];
                
                if (elementType == typeof(string)){
                    List<string> listValue = new List<string>();
                    
                    if (!string.IsNullOrEmpty(rawValue)){
                        if (rawValue.StartsWith("[") && rawValue.EndsWith("]")){
                            // JSON array format
                            try{
                                string jsonWrapped = "{\"items\":" + rawValue + "}";
                                StringArrayWrapper wrapper = JsonUtility.FromJson<StringArrayWrapper>(jsonWrapped);
                                listValue = wrapper.items?.ToList() ?? new List<string>();
                            }
                            catch (Exception parseEx){
                                Debug.LogWarning($"{DebugID} SetPlayerDataField: JSON array parsing failed for '{rawValue}': {parseEx.Message}. Attempting fallback comma-split.");
                                string inner = rawValue.Substring(1, rawValue.Length - 2).Trim();
                                if (!string.IsNullOrEmpty(inner)){
                                    listValue = inner.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(s => s.Trim().Trim('"')).ToList();
                                }
                            }
                        }
                        else{
                            // Comma-separated format
                            listValue = rawValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(s => s.Trim()).ToList();
                        }
                    }
                    
                    targetField.SetValue(_playerData, listValue);
                    return true;
                }
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
