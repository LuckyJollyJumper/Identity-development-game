using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that manages the quests. It has a list of all available quests and tracks the progress of the current quests.
/// It also handles the tutorial. The AvailableQuests list is synced with the PlayerData quests.
/// ONLY USED FOR ONE QUEST AS OF NOW
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
    [Header("References")]
    [SerializeField] public GuidancePointer GuidancePointer;

    [Header("Variables")]
    [Tooltip("List of all current quests and corresponding Objects that start them if they exist, synced with PlayerData")]
    [SerializeField] public List<(QuestData,GameObject)> AvailableQuests = new(); //GameObject is used for future interactions

    [Header("Debug")]
    [SerializeField] private bool DebugMode = false;
    [SerializeField] private readonly string DebugID = "[QuestManager]";

    /// <summary>
    /// Used by an object n the world to add a quest to the list of quests. Preferably done at the start of the game
    /// </summary>
    public void AddQuestData(QuestData quest, GameObject NPC){
        AvailableQuests.Add((quest,NPC));
    }

    /// <summary>
    /// Updates the progress of a quest by amount. If the progress reaches the goal, it will invoke the signal
    /// in the quest that should be linked to the NPC as set in SetQuestAsActive()
    /// </summary>
    public void UpdateProgress(int amount, string questName){
        for (int i = 0; i < AvailableQuests.Count; i++){
            QuestData quest = AvailableQuests[i].Item1;
            if (quest.QuestName == questName){
                quest.CurrentProgress += amount;
                if (DebugMode){ Debug.Log($"{DebugID} Quest {quest.QuestName} progress updated: {quest.CurrentProgress}/{quest.Goal}"); }
                if (quest.CurrentProgress >= quest.Goal){ quest.CompleteQuest(); }
                return;
            }
        }
        if (DebugMode){ Debug.Log($"{DebugID} Quest \"{questName}\" not found to be updated"); }
    }

    /// <summary>
    /// Used by the NPC's to activate their quests.
    /// </summary>
    /// <param name="questName"></param>
    public void SetQuestAsActive(string questName){
        for (int i = 0; i < AvailableQuests.Count; i++){
            QuestData quest = AvailableQuests[i].Item1;
            if (quest.QuestName == questName){
                quest.IsActive = true;
                if (DebugMode) Debug.Log($"{DebugID} Quest {quest.QuestName} set as active.");
                return;
            }
        }
    }
    public QuestData GetQuestData(string questName){
        var (quest, _) = AvailableQuests.Find(q => q.Item1.QuestName == questName);
        return quest;
    }
}
