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
    /// Used to display the welcome message and initiates the tutorial.
    /// Only used on first visit of the game.
    /// </summary>
    public void StartLvl0Tutorial(List<AudioClip> audioClips){
        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Welkom op je nieuwe school!\n\nRaak ergens het scherm aan om door te gaan.",
            $"Je kan rondlopen door de witte cirkel beneden op het scherm te verplaatsen"
        };
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpAudioClips = audioClips;
        GameObject popupInstance = Instantiate(PopupWindowPrefab, this.transform);
        //TODO: add first tutorial to all objects
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
