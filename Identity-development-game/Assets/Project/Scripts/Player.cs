using UnityEngine;
using System.IO;

/// <summary>
/// Player data class to hold player-related information during runtime.  
/// </summary>

public class Player : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public string playerName;
    [HideInInspector] public PlayerData playerData;
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

    public void LoadPlayerData(){
        if (File.Exists(savePath)){
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Loaded: Name {data.playerName}");
        }
        else{ Debug.LogWarning("No save file found!"); }
    }
}
