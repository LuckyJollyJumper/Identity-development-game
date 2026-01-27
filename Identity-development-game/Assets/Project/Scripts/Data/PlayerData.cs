using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Player data class to hold player-related information during runtime and for saving/loading.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData: ScriptableObject
{
    // public PlayerData(){
    //     PlayerName              = "NewPlayer";
    //     Level                   = 1; // Progression in quest
    //     Points                  = 0;
    //     Coins                   = 0;
    //     Progress                = new List<ActivityData>();
    //     Inventory               = new List<ItemData>();
    //     CharacterStyle          = new List<string>();
    //     SelectedPersonas1       = new List<string>();
    //     SelectedPersonas2       = new List<string>();
    //     SelectedPersonas3       = new List<string>();
    //     SelectedPersonas4       = new List<string>();
    //     SelectedPersonas5       = new List<string>();
    //     SelectedPersonas6       = new List<string>();
    //     SelectedPersonasSchool  = new List<string>();
    // }
    public string PlayerName;
    public int Level;
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

    public void AddPoints(ActivityData activity){
       Progress.Add(activity);
       Points += activity.ActivityPoints;
    }
}








public class JsonSaveSystem
{
    [HideInInspector] public string PlayerSavePath;
    [HideInInspector]public string ServerSavePath;
    private string DebugID = "[JsonSaveSystem]";
    public bool DebugMode = true;

    public JsonSaveSystem(){
        Debug.Log(Application.persistentDataPath);
        PlayerSavePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
        ServerSavePath = Path.Combine(Application.persistentDataPath, "serverSave.json");
    }
    
    //----------------------\\
    // Player data methods
    //----------------------\\
    public void SavePlayerData(PlayerData player){
        try{
            string json = JsonUtility.ToJson(player, true);
            File.WriteAllText(PlayerSavePath, json);
            
            if(DebugMode){ Debug.Log($"{DebugID} Player data saved to {PlayerSavePath}"); }
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to save player data: {e.Message}");
        }
    }

    // returns (isExistingPlayer, PlayerData)
    public (bool, PlayerData) LoadPlayerData(){
        if(!File.Exists(PlayerSavePath)){
            if(DebugMode){Debug.Log($"{DebugID} No save file found at {PlayerSavePath}");}
            return (false, ScriptableObject.CreateInstance<PlayerData>());
        }

        string json = File.ReadAllText(PlayerSavePath);
        PlayerData loadedPlayer = JsonUtility.FromJson<PlayerData>(json);
        
        if(DebugMode){Debug.Log($"{DebugID} Player data loaded from {PlayerSavePath}");}
        
        return (true, loadedPlayer);
    }

    public void DeleteSaveData(){
        try{
            if(File.Exists(PlayerSavePath)){
                File.Delete(PlayerSavePath);
                
                if(DebugMode){ Debug.Log($"{DebugID} Player save data deleted from {PlayerSavePath}");}
            }
            else 
            if(DebugMode){ Debug.Log($"{DebugID} No save file found at {PlayerSavePath}");}
        }
        catch (System.Exception e){
            Debug.LogError($"{DebugID} Failed to delete save data: {e.Message}");
        }
    }

    //----------------------\\
    // Server data methods
    //----------------------\\
    public ServerData LoadServerData(){
        return ScriptableObject.CreateInstance<ServerData>();
    }
}
