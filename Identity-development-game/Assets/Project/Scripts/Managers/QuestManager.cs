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
    [HideInInspector] public event System.Action OnProgressCompleted;

    public void Start(){
        SchoolMapCanvas ??= FindFirstObjectByType<UISchoolMap>();
        GuidancePointer ??= FindFirstObjectByType<GuidancePointer>();

        this.AvailableQuests = new List<(QuestData,GameObject)>(){
            (new(){
                QuestName = "Verloren boeken",
                Description = "Vind alle boeken voor Axel die verspreid liggen in de school",
                Goal = 8,
                CurrentProgress = 0,
                RewardCoins = 2,
                RewardPoints = 300,
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
            GameManager.Instance._playerData.LevelUp();
            GameManager.Instance.SaveGame();
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

    public QuestData GetQuestData(string questName){
        foreach (var (quest, _) in AvailableQuests){
            if (quest.QuestName == questName){
                return quest;
            }
        }
        return null;
    }

    public void UpdateProgress(int amount, QuestData quest){
        quest.CurrentProgress += amount;
        if (quest.CurrentProgress >= quest.Goal){
            this.OnProgressCompleted?.Invoke(); // Invoke the event to notify the character that the quest has been completed
        }
    }

    /// <summary>
    /// Used by the NPC's to activate their quests
    /// </summary>
    /// <param name="questName"></param>
    public void SetQuestAsActive(string questName, GameObject NPC){
        foreach (var (quest, npc) in AvailableQuests){
            if (quest.QuestName == questName){
                quest.IsActive = true;
                npc = NPC;
                return;
            }
        }
    }
}
