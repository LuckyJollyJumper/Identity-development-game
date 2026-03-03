using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The base of calling and starting the tutorial sequence.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Tooltip("The objects that are to be excluded when the tutorial starts, e.g. the main UI, so that the player can only interact with the tutorial pop-ups and not the rest of the UI")]
    [SerializeField ] private GameObject TutorialObjects;
    [SerializeField] public UISchoolMap SchoolMapCanvas;
    [Header("Audio Clips")]
    [SerializeField] public List<AudioClip> WelcomeAudios1;
    [Header("Debug")]
    [SerializeField] private bool DebugMode = true;
    [SerializeField] private bool AlwaysStartTutorial = false;
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
        if (GameManager.Instance._playerData.Level == 0 || AlwaysStartTutorial){
            TutorialObjects.SetActive(false);
            TutorialQuest.IsActive = true;
            QuestManager.Instance.AddQuestData(TutorialQuest, this.gameObject);
            SchoolMapCanvas.StartStep1();
            // Subscribe to set the onCompleted event of the whole tutorial
            TutorialQuest.OnQuestCompleted += () => {
                GameManager.Instance.LevelUpPlayer();
                TutorialObjects.SetActive(true);
                if (DebugMode){ Debug.Log($"{DebugID} Tutorial Quest completed!"); }
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
            $"Volg de pijl naast je karakter om verder te gaan!"
        };
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpAudioClips = WelcomeAudios1;
        PopupWindowPrefab.GetComponent<PopUpWindow>().OnPopUpClosed += () => {
            QuestManager.Instance.UpdateQuestProgress(1, QuestName);
            StartStep2();
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, SchoolMapCanvas.transform);
    }

    public void StartStep2(){
        // wait four seconds before showing the next popup; Timer.Delay is a static
        // coroutine provided by the Timer utility.
        StartCoroutine(Step2Coroutine());
    }

    private System.Collections.IEnumerator Step2Coroutine(){
        yield return Timer.Delay(4f);   // pause here for four seconds

        if (DebugMode){ Debug.Log($"{DebugID} Starting tutorial step 2: Movement tutorial"); }
        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Goed zo!"
        };
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpAudioClips = WelcomeAudios1;
        PopupWindowPrefab.GetComponent<PopUpWindow>().OnPopUpClosed += () => {
            QuestManager.Instance.UpdateQuestProgress(1, QuestName);
            StartStep2();
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, SchoolMapCanvas.transform);
    }

    private void StartStep3(){
        if (DebugMode){ Debug.Log($"{DebugID} Starting tutorial step 3: Interaction tutorial"); }
        GameObject PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Goed zo! Objecten in de wereld met een witte wolk erboven zijn interactief.",
            $"Volg de pijl naast je karakter om met je eerste object te interacteren."

    private void OnTriggerEnter(Collider collision) {
        if (DebugMode){ Debug.Log($"{DebugID} Collision detected in TutorialManager"); }
        if (collision.gameObject.CompareTag("Player")){
            // Trigger the tutorial pop-up or sequence here
            if (DebugMode){ Debug.Log($"{DebugID} Player has entered the tutorial area!"); }
        }
    }
   
}
