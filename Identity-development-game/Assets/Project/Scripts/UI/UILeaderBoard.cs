using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Used by Orderby
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
    [SerializeField] public GameObject Parent;
    private GameObject Grid;
    
    void Start(){
        this.Grid = transform.Find("BackgroundImage/Content").gameObject;
        UpdateLeaderBoard();
    }

    /// <summary>
    /// Updates the leaderboard by looking at the server data and sorting the players by points. 
    /// Also highlights the player on this device. Called on start of the leaderboard and can be 
    /// called from other scripts to update the leaderboard after changes to player data.
    /// </summary>
    public void UpdateLeaderBoard(){
        List<PlayerData> players = GameManager.Instance.GetServerData().Players;
        players = players.OrderByDescending(p=>p.Points).ToList();
        int index = 1;
        foreach (PlayerData p in players){
            GameObject leaderboardPrefab = Resources.Load<GameObject>("LeaderBoardItem");
            leaderboardPrefab.GetComponent<LeaderBoardItem>().SetValues(index, "", p.PlayerName, p.Points);
            // Highlight the player on this device
            if (p.PlayerID == GameManager.Instance._playerData.PlayerID){
                leaderboardPrefab.GetComponent<Image>().color = new Color(0.129f, 0.623f, 1f);
            }
            Instantiate(leaderboardPrefab, Grid.transform);
            index++;
            // Why do I need to do this?
            leaderboardPrefab.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
       
    }

    /// <summary>
    /// Function used by the Quitbutton in the UI
    /// </summary>
    public void CloseLeaderBoard(){
        Parent.GetComponent<InteractableObject>().OnEndInteract();
    }
}
