using UnityEngine;
using TMPro;

/// <summary>
/// UI class that controls all the UI elements for the player Hud at the top of the screen.
/// Mainly used to distribute data to it
/// </summary>
public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI PlayerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI PlayerPointsText;
    [SerializeField] private TMPro.TextMeshProUGUI PlayerCoinsText;
    [SerializeField] private GameObject Inventory;

    public void Awake(){
        foreach (TMPro.TextMeshProUGUI t in GetComponentsInChildren<TMPro.TextMeshProUGUI>()){
            if (t.name == "NameText"){
                PlayerNameText = t;
                continue;
            }
            if (t.name == "LvlText"){
                PlayerPointsText = t;
                continue;
            }
            if (t.name == "CoinText"){
                PlayerCoinsText = t;
                continue;
            }
        }

    }

    public void DisplayPlayerData(PlayerData data){
        PlayerNameText.text = data.PlayerName;
        PlayerPointsText.text = $"Pts {data.Points.ToString()}";
        PlayerCoinsText.text = data.Coins.ToString();
    }

    public void OpenCloseInventory(){
        if (Inventory.activeSelf){
            Inventory.SetActive(false);
        }
        else{
            Inventory.SetActive(true);
        }
    }

}
