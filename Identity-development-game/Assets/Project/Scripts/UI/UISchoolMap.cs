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

    public void StartLvl0Tutorial(){
        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Welkom op je nieuwe school!\n\nRaak ergens het scherm aan om door te gaan.",
            $"Je kan rondlopen door de witte cirkel beneden op het scherm te verplaatsen\n\nVeel Plezier met spelen!"
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, this.transform);
    }

    public void DisplayPlayerData(PlayerData player){
        playerHud.DisplayPlayerData(player);
    }
}
