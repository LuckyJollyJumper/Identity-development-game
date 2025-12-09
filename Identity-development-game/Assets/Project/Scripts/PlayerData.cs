using UnityEngine;
using System.IO;

/// <summary>
/// Player data class to hold player-related information during runtime and for saving/loading.
/// </summary>


public class PlayerData
{
    public PlayerData(){
        playerName = "NewPlayer";
        level = 1;
        exp = 0;
        coins = 0;
        progress = 0;
        inventory = new string[] { };
        characterStyle = new string[] { };
    }
    public string playerName;
    public int level;
    public int exp;
    public int coins;
    public int progress;
    public string[] inventory;
    public string[] characterStyle;
}



public class JsonSaveSystem
{
    [HideInInspector] public string savePath;
    private string ID = "[JsonSaveSystem]";

    public JsonSaveSystem(){
        savePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    public void SavePlayerData(PlayerData player){
        string json = JsonUtility.ToJson(player, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"{ID} Player saved to: {savePath}");
    }

    // returns (isExistingPlayer, PlayerData)
    public (bool, PlayerData) LoadPlayerData(){
        if (File.Exists(savePath)){
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"{ID} Loaded: Name {data.playerName}");
            return (false, data);
        }
        else{  
            File.WriteAllText(savePath, JsonUtility.ToJson(new PlayerData(), true));
            Debug.Log($"{ID} No savefile found. New player saved to: {savePath}");
            return (true, new PlayerData());
        }
    }

    public void DeleteSaveData(){
        if (File.Exists(savePath)){
            File.Delete(savePath);
            Debug.Log($"{ID} Save file deleted.");
        }
        else{ Debug.LogWarning($"{ID} No save file to delete."); }
    }

}
