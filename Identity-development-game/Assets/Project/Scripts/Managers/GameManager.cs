 using UnityEngine;
using Assets.Project.Scripts.Player;

public class GameManager : MonoBehaviour
{
    public Player player;
    public static ScenesManager scenesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake(){
        DontDestroyOnLoad(this.gameObject);
        (bool newPlayer, player) = JsonSaveSystem.LoadPlayerData();
        if (newPlayer){
            scenesManager.LoadScene(ScenesManager.scenes.CharacterCreator);
            Debug.Log("New player created");
        }
        else{
            //TODO: load all data in apropriate places
            Debug.Log("Player:" + player.playerData.playerName + " loaded");
        }

    }

}
