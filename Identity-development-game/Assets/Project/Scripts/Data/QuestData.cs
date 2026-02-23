using UnityEngine;

[System.Serializable]
public class QuestData
{
    public bool IsActive; // Whether the quest is currently active for the player
    public string QuestName;
    public string Description; 
    public Sprite QuestImage;
    public int RewardCoins;
    public int RewardPoints;
    // Values that track the collection or progress of a generic action
    public int CurrentProgress = 0;
    public int Goal;

    public bool IsCompleted(){
        return CurrentProgress >= Goal;
    }
}
