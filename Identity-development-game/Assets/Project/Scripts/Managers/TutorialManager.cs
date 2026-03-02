using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The base of calling and starting the tutorial sequence.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField] public UISchoolMap SchoolMapCanvas;
    [Header("Audio Clips")]
    [SerializeField] public List<AudioClip> WelcomeAudios1;
    [Header("Debug")]
    [SerializeField] private bool DebugMode = true;
    private string DebugID = "[TutorialManager]";
    public QuestData TutorialQuest = new(){
        QuestName = "Tutorial",
        Description = "Called from TutorialManager, if IsActive, the ",
        Goal = 8,
        CurrentProgress = 0,
        RewardCoins = 0,
        RewardPoints = 0,
        IsActive = false
    };

    public void Start(){
        if (GameManager.Instance._playerData.Level == 0){
            TutorialQuest.IsActive = true;
            QuestManager.Instance.AddQuestData(TutorialQuest, this.gameObject);
            SchoolMapCanvas.StartLvl0Tutorial(WelcomeAudios1);
            TutorialQuest.OnQuestCompleted += () => {
                GameManager.Instance.LevelUpPlayer();
                if (DebugMode){ Debug.Log($"{DebugID} Tutorial Quest completed!"); }
            };
        }
        else{
            if (DebugMode){ Debug.Log($"{DebugID} Player level is {GameManager.Instance._playerData.Level}, skipping tutorial"); }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (DebugMode){ Debug.Log($"{DebugID} Collision detected in TutorialManager"); }
        if (collision.gameObject.CompareTag("Player")){
            // Trigger the tutorial pop-up or sequence here
            if (DebugMode){ Debug.Log($"{DebugID} Player has entered the tutorial area!"); }
        }
    }
   
}
