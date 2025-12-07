using UnityEngine;
using System.IO;

/// <summary>
/// Player data class to hold player-related information during runtime.  
/// </summary>

public class Player : MonoBehaviour
{
    [SerializeField] public Transform transform;
    [HideInInspector] public PlayerData playerData;

    public void Start(){
        this.transform = this.transform;
        this.playerData = new();
    }
}

public class PlayerData
{
    public string playerName;
    public int level;
    public int exp;
    public int coins;
    public int progress;
    public string[] inventory;
    public string[] characterStyle;
}



public class JsonSaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake(){
        savePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    public void SavePlayerData(PlayerData player){
        string json = JsonUtility.ToJson(player, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public (bool, PlayerData) LoadPlayerData(){
        if (File.Exists(savePath)){
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Loaded: Name {data.playerName}");
            return (true, data);
        }
        else{ 
             Debug.LogWarning("No save file found!"); 
            return (false, new Player());
        }
    }
    public void DeleteSaveData(){
        if (File.Exists(savePath)){
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
        else{ Debug.LogWarning("No save file to delete."); }
    }

}
