using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Player data class to hold player-related information during runtime and for saving/loading.
/// </summary>


public class PlayerData
{
    public PlayerData(){
        playerName = "NewPlayer";
        level = 1;
        points = 0;
        coins = 0;
        progress = new List<ActivityData>();
        inventory = new List<string>();
        characterStyle = new List<string>();
        selectedPersonas1 = new List<string>();
    }
    public string playerName;
    public int level;
    public int points;
    public int coins;
    public List<ActivityData> progress; // Stores activities the player has completed, in order
    public List<string> inventory;
    public List<string> characterStyle;
    public List<string> selectedPersonas1;

    public void AddPoints(ActivityData activity){
       progress.Add(activity);
       points += activity.activityPoints;
    }
}

public class ServerData{
    public List<PlayerData> players;
}



public class JsonSaveSystem
{
    [HideInInspector] public string playerSavePath;
    [HideInInspector]public string serverSavePath;
    private string DebugID = "[JsonSaveSystem]";
    public bool debugMode = true;

    public JsonSaveSystem(){
        playerSavePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
        serverSavePath = Path.Combine(Application.persistentDataPath, "serverSave.json");
    }
    
    // Player data methods

    public void SavePlayerData(PlayerData player){
        string json = JsonUtility.ToJson(player, true);
        File.WriteAllText(playerSavePath, json);
        if (debugMode){ Debug.Log($"{DebugID} Player saved to: {playerSavePath}"); }
    }

    // returns (isExistingPlayer, PlayerData)
    public (bool, PlayerData) LoadPlayerData(){
        if (File.Exists(playerSavePath)){
            string json = File.ReadAllText(playerSavePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            if (debugMode){ Debug.Log($"{DebugID} Loaded: Name {data.playerName}"); }
            return (false, data);
        }
        else{  
            SavePlayerData(new PlayerData());
            if (debugMode){ Debug.Log($"{DebugID} No savefile found. New player saved to: {playerSavePath}"); }
            return (true, new PlayerData());
        }
    }

    public void DeleteSaveData(){
        if (File.Exists(playerSavePath)){
            File.Delete(playerSavePath);
            if (debugMode){ Debug.Log($"{DebugID} Save file deleted."); }
        }
        else{ Debug.LogWarning($"{DebugID} No save file to delete."); }
    }

    // Server data methods

    public void SaveServerData(ServerData data){
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(serverSavePath, json);
        if (debugMode){ Debug.Log($"{DebugID} Server data saved to: {serverSavePath}"); }
    }

    public ServerData LoadServerData(){
        if (File.Exists(serverSavePath)){
            string json = File.ReadAllText(serverSavePath);
            ServerData data = JsonUtility.FromJson<ServerData>(json);
            if (debugMode){ Debug.Log($"{DebugID} Loaded server data from: {serverSavePath}"); }
            return data;
        }
        else{  
            ServerData newData = new ServerData();
            newData.players = new List<PlayerData>();
            SaveServerData(newData);
            if (debugMode){ Debug.Log($"{DebugID} No server savefile found. New server data saved to: {serverSavePath}"); }
            return newData;
        }
    }

}
