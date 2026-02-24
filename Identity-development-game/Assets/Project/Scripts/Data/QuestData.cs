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
    [field: System.NonSerialized] public event System.Action OnQuestCompleted;

    public bool IsCompleted(){
        return CurrentProgress >= Goal;
    }

    /// <summary>
    /// Invokes the OnQuestCompleted event to signal that the quest has been completed.
    /// Needs to be called inside its own class
    /// </summary>
    public void CompleteQuest(){
        OnQuestCompleted?.Invoke();
    }
}
