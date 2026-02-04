using UnityEngine;
using System;

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
        this.CompletionDate = new Date();
    }
    public enum ActivitiesType { Unconnected, Art, Sports, Woodworking }
    
    public string ActivityName;
    public ActivitiesType ActivityType;
    public int ActivityPoints; // Points awarded for completing the activity
    public float ActivityDuration; // Duration in seconds
    public int RewardCoins; // Amount of coins awarded for completing the activity
    public Date CompletionDate; // day / month / year
    
}


/// <summary>
/// Used to store day, month and year. Sets its own value on initialisation
/// </summary>
[System.Serializable]
public class Date
{
    public int Day;
    public int Month;
    public int Year;

    void Start(){
        this.Day    = DateTime.Now.Day; 
        this.Month  = DateTime.Now.Month;
        this.Year   = DateTime.Now.Year;
    }

    public void SetTimeNow(){
        Start();
    }
}
