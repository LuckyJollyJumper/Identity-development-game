using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The base of calling and starting the tutorial sequence.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The objects that are to be excluded when the tutorial starts, e.g. the main UI, so that the player can only interact with the tutorial pop-ups and not the rest of the UI")]
    [SerializeField] private GameObject TutorialObjects;
    [SerializeField] private Transform NPC;
    [SerializeField] private UISchoolMap SchoolMapCanvas;
    [SerializeField] private GuidancePointer PointerObject;

    [Header("Audio Clips")]
    [SerializeField] public List<AudioClip> WelcomeAudios1;

    [Header("Debug")]
    [SerializeField] private bool DebugMode = true;
    [SerializeField] private bool AlwaysStartTutorial = false;
    private string DebugID = "[TutorialManager]";
    public QuestData TutorialQuest = new(){
        QuestName = "Tutorial",
        Description = "Called from TutorialManager, if IsActive, the ",
        Goal = 3,
        CurrentProgress = 0,
        RewardCoins = 0,
        RewardPoints = 0,
        IsActive = false
    };

    public void Start(){
        if (GameManager.Instance._playerData.Level == 0 || AlwaysStartTutorial){
            TutorialObjects.SetActive(false);
            TutorialQuest.IsActive = true;
            QuestManager.Instance.AddQuestData(TutorialQuest, this.gameObject);
            StartStep1();
            // Subscribe to set the onCompleted event of the whole tutorial
            TutorialQuest.OnQuestCompleted += () => {
                GameManager.Instance.LevelUpPlayer();
                TutorialObjects.SetActive(true);
                if (DebugMode){ Debug.Log($"{DebugID} Tutorial Quest completed!"); }
                Destroy(this.gameObject);
            };
        }
        else{
            if (DebugMode){ Debug.Log($"{DebugID} Player level is {GameManager.Instance._playerData.Level}, skipping tutorial"); }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Used to display the welcome message and initiates the tutorial.
    /// Only used on first visit of the game.
    /// </summary>
    public void StartStep1(){
        if (DebugMode){ Debug.Log($"{DebugID} Starting tutorial step 1: Welcome message"); }

        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Welkom op je nieuwe school!\n\nRaak ergens het scherm aan om door te gaan.",
            $"Je kan rondlopen door de witte cirkel beneden op het scherm te verplaatsen",
            $"Volg de pijl bij je karakter om verder te gaan"
        };
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpAudioClips = WelcomeAudios1;
        PopupWindowPrefab.GetComponent<PopUpWindow>().OnPopUpClosed += () => {
            QuestManager.Instance.UpdateQuestProgress(1, TutorialQuest.QuestName);
            if (DebugMode){ Debug.Log($"{DebugID} Completed step 1"); }
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, SchoolMapCanvas.transform);
    }

    public void StartStep2(){
        if (DebugMode){ Debug.Log($"{DebugID} Starting tutorial step 2: Movement tutorial"); }
        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Goed zo! Objecten in de wereld met een witte wolk erboven zijn interactief",
            $"Dus elke wolk die je tegen komt kan je verder helpen in het spel",
            $"Probeer maar eens op de wolk met de tekst scorebord te klikken!"
        };
        // PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpAudioClips = WelcomeAudios1;
        PopupWindowPrefab.GetComponent<PopUpWindow>().OnPopUpClosed += () => {
            QuestManager.Instance.UpdateQuestProgress(1, TutorialQuest.QuestName);
            if (DebugMode){ Debug.Log($"{DebugID} Completed step 2"); }
            PointerObject.target = NPC;
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, SchoolMapCanvas.transform);
    }

    /// <summary>
    /// Used to trigger the second step of the tutorial when the player enters the collider.
    /// </summary>
    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player")){
            if (DebugMode){ Debug.Log($"{DebugID} Player has entered the tutorial area!"); }
            StartStep2();
            this.GetComponent<BoxCollider>().enabled = false; // Disable the collider so that the tutorial doesn't get triggered again
        }
    }
   
}
