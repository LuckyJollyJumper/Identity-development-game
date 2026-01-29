using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data class to simulate the server and having all the data of all the players
/// </summary>
[System.Serializable]
public class ServerData
{
    public List<PlayerData> Players;

    /// <summary>
    /// Checks whether the player (by name) already exists and updates that one. Otherwise it just 
    /// add it to the list
    /// </summary>
    /// <param name="player"></param>
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