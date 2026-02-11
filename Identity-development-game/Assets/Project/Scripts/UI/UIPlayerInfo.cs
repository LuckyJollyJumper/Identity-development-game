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
    [SerializeField] private TMPro.TextMeshProUGUI LvlText;
    [SerializeField] private GameObject Inventory;

    public void Awake(){
        foreach (TMPro.TextMeshProUGUI t in GetComponentsInChildren<TMPro.TextMeshProUGUI>()){
            switch (t.name){
                case "NameText": PlayerNameText = t; break;
                case "LvlText": PlayerPointsText = t; break;
                case "CoinText": PlayerCoinsText = t; break;
                case "LevelText": LvlText = t; break;
            }
        }
    }

    public void DisplayPlayerData(PlayerData data){
        PlayerNameText.text = data.PlayerName;
        PlayerPointsText.text = $"{data.Points.ToString()} pts";
        PlayerCoinsText.text = $"{data.Coins}<Sprite index=0>";
        LvlText.text = $"{data.Level}";
    }

    public void UpdateInventory(){ Inventory.GetComponent<UIInventory>().ReloadInventory(); }

    public void OpenCloseInventory(){ Inventory.SetActive(!Inventory.activeSelf); }

}
