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
    [SerializeField] public UISchoolMap SchoolMapCanvas;
    [SerializeField] public GuidancePointer GuidancePointer;

    [Header("Variables")]
    [Tooltip("List of all current quests and corresponding NPC's if they exist, synced with PlayerData")]
    [SerializeField] public List<(QuestData,GameObject)> AvailableQuests; 

    [Header("Debug")]
    [SerializeField] private bool DebugMode = false;
    [SerializeField] private readonly string DebugID = "[QuestManager]";

    public void Start(){
        SchoolMapCanvas ??= FindFirstObjectByType<UISchoolMap>();
        GuidancePointer ??= FindFirstObjectByType<GuidancePointer>();

        this.AvailableQuests = new List<(QuestData,GameObject)>(){
            (new(){
                QuestName = "Verloren boeken",
                Description = "Vind alle boeken voor Axel die verspreid liggen in de school",
                Goal = 8,
                CurrentProgress = 0,
                RewardCoins = 1,
                RewardPoints = 400,
                IsActive = false
            },
            null)
        };
        
    }


    /// <summary>
    /// Used to start the quest for the current level. For example it starts the tutorial for level 0.
    /// </summary>
    /// <param name="level"></param>
    public void StartLevelQuest(int level){
        if (level == 0){
            SchoolMapCanvas.StartLvl0Tutorial();
            GameManager.Instance.LevelUpPlayer();
        }
    }

    /// <summary>
    /// Used to update the progress of a quest. Returns the updated quest to the object that called it, so it can use the updated values for things like the pop-up text.
    /// </summary>
    /// <param name="questName"></param>
    /// <returns></returns>
    public QuestData FoundHiddenObject(string questName){
        foreach (var (quest, _) in AvailableQuests){
            if (quest.QuestName == questName){
                UpdateProgress(1, quest);
                return quest;
            }
        }
        return null;
    }

    /// <summary>
    /// Updates the progress of a quest by amount. If the progress reaches the goal, it will invoke the signal
    /// in the quest that should be linked to the NPC as set in SetQuestAsActive()
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="quest"></param>
    public void UpdateProgress(int amount, QuestData quest){
        quest.CurrentProgress += amount;
        if (quest.CurrentProgress >= quest.Goal){
            quest.CompleteQuest();
        }
    }

    /// <summary>
    /// Used by the NPC's to activate their quests. Sets
    /// </summary>
    /// <param name="questName"></param>
    public void SetQuestAsActive(string questName, GameObject NPC){
        for (int i = 0; i < AvailableQuests.Count; i++){
            QuestData quest = AvailableQuests[i].Item1;
            if (quest.QuestName == questName){
                quest.IsActive = true;
                quest.OnQuestCompleted += () => {
                    // This is the callback that will be called when the quest is completed. It can be used to give rewards or update the NPC's dialogue.
                    if (DebugMode) Debug.Log($"{DebugID} Quest {quest.QuestName} completed!");
                    NPC.GetComponent<InteractableQuestCharacter>().QuestCompleted(quest);
                };
                AvailableQuests[i] = (quest, NPC);
                if (DebugMode) Debug.Log($"{DebugID} Quest {quest.QuestName} set as active by NPC:{NPC.name}");
                return;
            }
        }
    }
    public QuestData GetQuestData(string questName){
        var (quest, _) = AvailableQuests.Find(q => q.Item1.QuestName == questName);
        return quest;
    }
    public (QuestData, GameObject) GetQuest(string questName){
        return AvailableQuests.Find(q => q.Item1.QuestName == questName);
    }
}
