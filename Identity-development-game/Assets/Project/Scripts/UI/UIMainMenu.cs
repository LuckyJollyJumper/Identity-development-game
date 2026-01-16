using UnityEngine;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] public TMPro.TextMeshProUGUI playerNameText;
    [SerializeField] public TMPro.TextMeshProUGUI playerlvlText;
    [SerializeField] public TMPro.TextMeshProUGUI playerPointsText;

    public void DisplayPlayerData(PlayerData data){
        playerNameText.text = data.playerName;
        playerlvlText.text = $"lvl {data.level.ToString()}";
        playerPointsText.text = data.coins.ToString();
    }

    public void DisplayServerData(ServerData data){
        // Implementation for displaying server data if needed
        // TODO
    }

    public void ContinueGame(){
        ScenesManager.Instance.LoadScene(ScenesManager.scenes.SchoolMap);
    }
}
