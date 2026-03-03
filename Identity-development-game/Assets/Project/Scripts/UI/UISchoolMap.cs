using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class used to manage everything in the UI in the schoolmap scene. For example popups
/// </summary>
public class UISchoolMap : MonoBehaviour
{
    [SerializeField] private UIPlayerInfo playerHud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        this.playerHud = FindFirstObjectByType<UIPlayerInfo>();
    }

    /// <summary>
    /// Reloads the inventory and all the players data in the HUD at the top of the screen.
    /// </summary>
    /// <param name="player"></param>
    public void UpdatePlayerHUD(PlayerData player){
        playerHud.DisplayPlayerData(player);
        playerHud.UpdateInventory();
    }
}
