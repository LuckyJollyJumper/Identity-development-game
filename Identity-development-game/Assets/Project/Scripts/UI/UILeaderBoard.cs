using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Used by Orderby
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
    private GameObject grid;
    public GameObject Parent;
    
    void Start(){
        this.grid = transform.Find("Content").gameObject;
        UpdateLeaderBoard();
    }

    public void UpdateLeaderBoard(){
        List<PlayerData> players = GameManager.Instance.GetServerData().Players;
        players.OrderBy(p=>p.Points).ToList();

        int index = 1;
        foreach (PlayerData p in players){
            GameObject leaderboardPrefab = Resources.Load<GameObject>("LeaderBoardItem");
            leaderboardPrefab.GetComponent<LeaderBoardItem>().SetValues(index, "", p.PlayerName, p.Points);
            // Highlight the player on this device
            if (p.PlayerName == GameManager.Instance._playerData.PlayerName){
                leaderboardPrefab.GetComponent<Image>().color = new Color(0.3551846f, 1f, 0f);
            }
            Instantiate(leaderboardPrefab, grid.transform);
            index++;
        }
       
    }

    /// <summary>
    /// Function used by the Quitbutton in the UI
    /// </summary>
    public void CloseLeaderBoard(){
        Parent.GetComponent<InteractableObject>().OnEndInteract();
    }
}
