using UnityEngine;
using System;

/// <summary>
/// UI class that controls all the UI elements for the profile of the player. Is included into the locker
/// interactable object.
/// </summary>
public class UIMainProfile : MonoBehaviour
{
    [SerializeField] public GameObject Locker;
    [SerializeField] private TMPro.TextMeshProUGUI PlayerLvlText;
    [SerializeField] private Progressbar LevelProgressbar;
    [SerializeField] private TMPro.TextMeshProUGUI TotalActivitiesText;
    [SerializeField] private TMPro.TextMeshProUGUI RecentActivitiesText;

    public void Start(){
        FillContent(GameManager.Instance._playerData);
    }
    /// <summary>
    /// Function used by the Quitbutton in the UI
    /// </summary>
    public void CloseLocker(){
        Locker.GetComponent<InteractableObject>().OnEndInteract();
    }

    /// <summary>
    /// Used to fill the UI with the data of the player
    /// </summary>
    /// <param name="player"></param>
    public void FillContent(PlayerData player){
        this.PlayerLvlText.text = $"Level {player.Level}";
        this.LevelProgressbar.SetProgress((float)player.Level / 10f);
        this.TotalActivitiesText.text = $"Alle activiteiten {player.Progress.Count}";
        this.RecentActivitiesText.text = $"Activiteiten deze week {player.Progress}";
    }
}
