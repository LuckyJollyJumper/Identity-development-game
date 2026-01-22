using UnityEngine;

public class ActivityData : ScriptableObject
{
    public enum ActivityType { Art, Sports, Woodworking }
    public string activityName;
    public ActivityType activityType;
    public int activityPoints; // Points awarded for completing the activity
    public float activityDuration; // Duration in seconds
    public int rewardCoins; // Amount of coins awarded for completing the activity
    
}
