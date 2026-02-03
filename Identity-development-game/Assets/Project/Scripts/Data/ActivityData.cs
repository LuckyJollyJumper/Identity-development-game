using UnityEngine;

/// <summary>
/// Data class used for storing results after completing activities
/// </summary>
[System.Serializable]
public class ActivityData
{
    public ActivityData(){
        this.ActivityName = "";
        this.ActivityType = ActivitiesType.Unconnected;
        this.ActivityPoints = 0;
        this.ActivityDuration = 0;
        this.RewardCoins = 0;
        this.CompletionDate = new Vector3(1f, 1f, 2026f);
    }
    public enum ActivitiesType { Unconnected, Art, Sports, Woodworking }
    
    public string ActivityName;
    public ActivitiesType ActivityType;
    public int ActivityPoints; // Points awarded for completing the activity
    public float ActivityDuration; // Duration in seconds
    public int RewardCoins; // Amount of coins awarded for completing the activity
    public Vector3 CompletionDate; // day / month / year
    
}
