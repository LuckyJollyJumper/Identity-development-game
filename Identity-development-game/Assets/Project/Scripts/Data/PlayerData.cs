using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Serializable data class for JSON serialization (JsonUtility cannot serialize ScriptableObjects directly).
/// </summary>
[System.Serializable]
public class PlayerDataSerialized
{
    public string PlayerName;
    public int Level;
    public int Points;
    public int Coins;
    public List<ActivityData> Progress;
    public List<ItemData> Inventory;
    public List<string> CharacterStyle;
    public List<string> SelectedPersonas1;
    public List<string> SelectedPersonas2;
    public List<string> SelectedPersonas3;
    public List<string> SelectedPersonas4;
    public List<string> SelectedPersonas5;
    public List<string> SelectedPersonas6;
    public List<string> SelectedPersonasSchool;
}


/// <summary>
/// Player data class to hold player-related information during runtime and for saving/loading.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData: ScriptableObject
{
    public string PlayerName;
    public int Level; // Corresponds to where you are in the games progression
    public int Points;
    public int Coins;
    public List<ActivityData> Progress; // Stores activities the player has completed, in order
    public List<ItemData> Inventory;
    public List<string> CharacterStyle;
    public List<string> SelectedPersonas1;
    public List<string> SelectedPersonas2;
    public List<string> SelectedPersonas3;
    public List<string> SelectedPersonas4;
    public List<string> SelectedPersonas5;
    public List<string> SelectedPersonas6;
    public List<string> SelectedPersonasSchool;

    public void AddActivityData(ActivityData activity){
       this.Progress.Add(activity);
       this.Points += activity.ActivityPoints;
       this.Coins += activity.RewardCoins;
    }

    public void LevelUp(){
        this.Level += 1;
    }

}







/// <summary>
/// Used to save and load from disk the player and server data
/// </summary>
public class JsonSaveSystem
{
    [HideInInspector] public string PlayerSavePath;
    [HideInInspector]public string ServerSavePath;
    private string DebugID = "[JsonSaveSystem]";
    public bool DebugMode = true;

    public JsonSaveSystem(){
        PlayerSavePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
        ServerSavePath = Path.Combine(Application.persistentDataPath, "serverSave.json");
    }
    
    //--------------------------------------------------------------------------------------------\\
    //                                    Player data methods
    //--------------------------------------------------------------------------------------------\\

    public PlayerDataSerialized SerialisePlayerData(PlayerData player){
        PlayerDataSerialized serialisedData = new(){
            PlayerName              = player.PlayerName,
            Level                   = player.Level,
            Points                  = player.Points,
            Coins                   = player.Coins,
            Progress                = player.Progress,
            Inventory               = player.Inventory,
            CharacterStyle          = player.CharacterStyle,
            SelectedPersonas1       = player.SelectedPersonas1,
            SelectedPersonas2       = player.SelectedPersonas2,
            SelectedPersonas3       = player.SelectedPersonas3,
            SelectedPersonas4       = player.SelectedPersonas4,
            SelectedPersonas5       = player.SelectedPersonas5,
            SelectedPersonas6       = player.SelectedPersonas6,
            SelectedPersonasSchool  = player.SelectedPersonasSchool
        };
        return serialisedData;
    }
    public PlayerData DeSerialisePlayerData(PlayerDataSerialized serialisedData){
        PlayerData loadedPlayer = ScriptableObject.CreateInstance<PlayerData>();
        loadedPlayer.PlayerName             = serialisedData.PlayerName;
        loadedPlayer.Level                  = serialisedData.Level;
        loadedPlayer.Points                 = serialisedData.Points;
        loadedPlayer.Coins                  = serialisedData.Coins;
        loadedPlayer.Progress               = serialisedData.Progress               ?? new List<ActivityData>();
        loadedPlayer.Inventory              = serialisedData.Inventory              ?? new List<ItemData>();
        loadedPlayer.CharacterStyle         = serialisedData.CharacterStyle         ?? new List<string>();
        loadedPlayer.SelectedPersonas1      = serialisedData.SelectedPersonas1      ?? new List<string>();
        loadedPlayer.SelectedPersonas2      = serialisedData.SelectedPersonas2      ?? new List<string>();
        loadedPlayer.SelectedPersonas3      = serialisedData.SelectedPersonas3      ?? new List<string>();
        loadedPlayer.SelectedPersonas4      = serialisedData.SelectedPersonas4      ?? new List<string>();
        loadedPlayer.SelectedPersonas5      = serialisedData.SelectedPersonas5      ?? new List<string>();
        loadedPlayer.SelectedPersonas6      = serialisedData.SelectedPersonas6      ?? new List<string>();
        loadedPlayer.SelectedPersonasSchool = serialisedData.SelectedPersonasSchool ?? new List<string>();

        return loadedPlayer;
    }

    /// <summary>
    /// Saves the current playerData to own device and server
    /// </summary>
    public void SavePlayerData(PlayerData player){
        try{
            // Convert PlayerData to serializable format
            PlayerDataSerialized serialisedData = SerialisePlayerData(player);
            
            // Save to own device
            string json = JsonUtility.ToJson(serialisedData, true);
            File.WriteAllText(PlayerSavePath, json);

            // Save playerdata to server
            SavePlayerDataToServer(player);
            
            if(DebugMode){ Debug.Log($"{DebugID} Player data saved to {PlayerSavePath} and synced with server"); }
        }
        catch (System.Exception e){Debug.LogError($"{DebugID} Failed to save player data: {e.Message}");}
    }

    /// <summary>
    /// Returns the playerData saved on own device if it is there. If not then a new playerData will be created
    /// returns (isExistingPlayer, PlayerData)
    /// </summary>
    public (bool, PlayerData) LoadPlayerData(){
        if(!File.Exists(PlayerSavePath)){
            if(DebugMode){Debug.Log($"{DebugID} No save file found at {PlayerSavePath}");}
            return (false, ScriptableObject.CreateInstance<PlayerData>());
        }

        try {
            string json = File.ReadAllText(PlayerSavePath);
            PlayerDataSerialized serialisedData = JsonUtility.FromJson<PlayerDataSerialized>(json);
            
            // Convert serialized data to PlayerData ScriptableObject
           PlayerData loadedPlayer = DeSerialisePlayerData(serialisedData);
            
            if(DebugMode){Debug.Log($"{DebugID} Player data \"{loadedPlayer.PlayerName}\" loaded from {PlayerSavePath}");}
            
            return (true, loadedPlayer);
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to load player data: {e.Message}. Overriding with new data");
            return (false, ScriptableObject.CreateInstance<PlayerData>());
        }
    }

    ///<summary>
    /// Deletes the current playerData on own device and will also remove it from the server
    /// <summary>
    public void DeletePlayerSaveData(){
        try{
            if(File.Exists(ServerSavePath)){
                DeletePlayerFromServer(GameManager.Instance._playerData);
            }
            if(File.Exists(PlayerSavePath)){
                File.Delete(PlayerSavePath);
                
                if(DebugMode){ Debug.Log($"{DebugID} Local player save data deleted from {PlayerSavePath}");}
            }
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to delete save data: {e.Message}");
        }
    }


    //--------------------------------------------------------------------------------------------\\
    //                                    Server data methods
    //--------------------------------------------------------------------------------------------\\

    /// <summary>
    /// Tries to load data from the server. 
    /// FOR PROTOTYPE ONLY: Will create a new server if none was found
    /// </summary>
    public ServerData LoadServerData(){
        if(!File.Exists(ServerSavePath)){
            if(DebugMode){Debug.Log($"{DebugID} No server save file found at {ServerSavePath}");}
            ServerData newServerData = ScriptableObject.CreateInstance<ServerData>();
            newServerData.Players = new List<PlayerData>();
            return newServerData;
        }

        try {
            string json = File.ReadAllText(ServerSavePath);
            ServerDataSerialized serialisedData = JsonUtility.FromJson<ServerDataSerialized>(json);
            
            // Convert serialized data to ServerData ScriptableObject
            ServerData loadedServerData = ScriptableObject.CreateInstance<ServerData>();
            loadedServerData.Players = new List<PlayerData>();
            
            if (serialisedData.Players != null){
                foreach (var serialisedPlayer in serialisedData.Players){
                    PlayerData playerData = DeSerialisePlayerData(serialisedPlayer);
                    loadedServerData.Players.Add(playerData);
                }
            }
            
            if(DebugMode){Debug.Log($"{DebugID} Server data loaded from {ServerSavePath}");}
            
            return loadedServerData;
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to load server data: {e.Message}. Creating new one");
            ServerData defaultServerData = ScriptableObject.CreateInstance<ServerData>();
            defaultServerData.Players = new List<PlayerData>();
            return defaultServerData;
        }
    }

    public void SaveServerData(ServerData serverData){
        try{
            // Convert ServerData to serializable format
            ServerDataSerialized serializedData = new(){
                Players = new List<PlayerDataSerialized>()
            };
            
            if (serverData.Players != null){
                foreach (var player in serverData.Players){
                    PlayerDataSerialized serializedPlayer = SerialisePlayerData(player);
                    serializedData.Players.Add(serializedPlayer);
                }
            }
            
            string json = JsonUtility.ToJson(serializedData, true);
            File.WriteAllText(ServerSavePath, json);
            
            if(DebugMode){ Debug.Log($"{DebugID} Server data saved to {ServerSavePath}"); }
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to save server data: {e.Message}");
        }
    }

    public void SavePlayerDataToServer(PlayerData player){
        GameManager.Instance._serverData = LoadServerData(); // Reload serverData
        GameManager.Instance._serverData.AddPlayerData(player);
        Debug.Log($"{DebugID} ServerData to be saved{ GameManager.Instance._serverData.Players[0].PlayerName}");
        SaveServerData(GameManager.Instance._serverData);
        if (DebugMode){ Debug.Log($"{DebugID} Synced player with server"); }
    }

    public void DeletePlayerFromServer(PlayerData player){
        //TODO: 
    }
}
