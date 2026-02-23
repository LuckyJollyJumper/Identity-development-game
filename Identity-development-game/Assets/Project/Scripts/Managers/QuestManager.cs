using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that manages the quests. It has a list of all available quests and tracks the progress of the current quests.
/// It also handles the global quests and tutorial
/// NOT FULLY IMPLEMENTED YET, JUST A BASE TO BUILD ON
/// </summary>
public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance{ // Make sure only one instance of QuestManager exists
        get{
            if (instance == null){
                instance = FindFirstObjectByType<QuestManager>();

                if (instance == null){
                    GameObject singleton = new GameObject(typeof(QuestManager).ToString());
                    instance = singleton.AddComponent<QuestManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }

    List<QuestData> AvailableQuests; // List of all current quests

    public event System.Action OnProgressCompleted;

    public void UpdateProgress(int amount, QuestData quest){
        quest.CurrentProgress += amount;
        if (quest.CurrentProgress >= quest.Goal){
            quest.CurrentProgress = quest.Goal;
            this.OnProgressCompleted?.Invoke();
        }
    }

    public bool IsQuestCompleted(QuestData quest){
        return quest.CurrentProgress >= quest.Goal;
    }
}
