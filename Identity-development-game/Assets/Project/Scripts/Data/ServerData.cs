using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data class to simulate the server and having all the data of all the players
/// </summary>
[CreateAssetMenu(fileName = "ServerData", menuName = "Scriptable Objects/ServerData")]
public class ServerData : ScriptableObject
{
    public List<PlayerData> players;

}
