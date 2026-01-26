using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Player data class to hold player-related information during runtime and for saving/loading.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData: ScriptableObject
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
       points += activity.ActivityPoints;
    }
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
    
    //----------------------\\
    // Player data methods
    //----------------------\\
    public void SavePlayerData(PlayerData player){
        try{
            string json = JsonUtility.ToJson(player, true);
            File.WriteAllText(playerSavePath, json);
            
            if(debugMode){ Debug.Log($"{DebugID} Player data saved to {playerSavePath}"); }
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to save player data: {e.Message}");
        }
    }

    // returns (isExistingPlayer, PlayerData)
    public (bool, PlayerData) LoadPlayerData(){
        if(!File.Exists(playerSavePath)){
            if(debugMode){Debug.Log($"{DebugID} No save file found at {playerSavePath}");}
            return (false, new PlayerData());
        }

        string json = File.ReadAllText(playerSavePath);
        PlayerData loadedPlayer = JsonUtility.FromJson<PlayerData>(json);
        
        if(debugMode){Debug.Log($"{DebugID} Player data loaded from {playerSavePath}");}
        
        return (true, loadedPlayer);
    }

    public void DeleteSaveData(){
        try{
            if(File.Exists(playerSavePath)){
                File.Delete(playerSavePath);
                
                if(debugMode){ Debug.Log($"{DebugID} Player save data deleted from {playerSavePath}");}
            }
            else 
            if(debugMode){ Debug.Log($"{DebugID} No save file found at {playerSavePath}");}
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to delete save data: {e.Message}");
        }
    }

    //----------------------\\
    // Server data methods
    //----------------------\\
    public ServerData LoadServerData(){
        return new();
    }
}
