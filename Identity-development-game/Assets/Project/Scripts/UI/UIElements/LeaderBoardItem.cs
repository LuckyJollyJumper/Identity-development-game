using UnityEngine;

/// <summary>
/// Script used to set values in one of the leaderboard entries to dynamically load the players
/// onto the leaderboard in order of points.
/// </summary>
public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI PositionText;
    [SerializeField] private GameObject AvatarSlot;
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private TMPro.TextMeshProUGUI PointsText;
    
    void Start(){
        this.PositionText   = transform.Find("PositionText").GetComponent<TMPro.TextMeshProUGUI>();
        this.AvatarSlot     = transform.Find("AvatarImage").gameObject;
        this.NameText       = transform.Find("NameText").GetComponent<TMPro.TextMeshProUGUI>();
        this.PointsText     = transform.Find("PointsText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    /// <summary>
    /// Used to set all its values. With the first 3 items getting a medal icon instead of just
    /// a number at positionText.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="avatar">Should be used for the icon. Not used now</param>
    /// <param name="name"></param>
    /// <param name="points"></param>
    public void SetValues(int position, string avatar, string name, int points){
        this.PositionText.text = position switch{
            1 => "<sprite=\"MedalsText\" index=8>",
            2 => "<sprite=\"MedalsText\" index=4>",
            3 => "<sprite=\"MedalsText\" index=0>",
            _ => $"{position}",
        };
        this.NameText.text = name;
        this.PointsText.text = $"{points}";
    }
}
