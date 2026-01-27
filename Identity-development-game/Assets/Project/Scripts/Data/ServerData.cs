using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data class to simulate the server and having all the data of all the players
/// </summary>
[CreateAssetMenu(fileName = "ServerData", menuName = "Scriptable Objects/ServerData")]
public class ServerData : ScriptableObject
{
    public List<PlayerData> Players;

    public void AddPlayerData(PlayerData player){
        int index = Players.FindIndex(p => p.PlayerName == player.PlayerName);
        if (index >= 0) {
            this.Players[index] = player;
        }
        // Player has not been saved to server yet
        else{
            this.Players.Add(player);
        }
    }

}
