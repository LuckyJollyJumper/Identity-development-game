using UnityEngine;
using System.IO;

/// <summary>
/// Player data class to hold player-related information during runtime.  
/// </summary>

public class Player : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public string playerName;
    private float exp;
    private int level;
    private float coins;
    
}

public class PlayerData
{
    public int level;
    public float coins;
    public string playerName;
}



public class JsonSaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "playerSave.json");
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData()
        {
            level = 5,
            coins = 200,
            playerName = "Rahul"
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Loaded: Level {data.level}, Coins {data.coins}, Name {data.playerName}");
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }
}
