using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manager class to handle the puzzle game from start to finish.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string Puzzlename = "Art Puzzle";
    [SerializeField] public ActivityData.ActivitiesType PuzzleType;
    [SerializeField] public int RewardCoins = 1;
    [HideInInspector] public List<DropLocation> DropLocations;
    [HideInInspector] public Timer PuzzleTimer;
    [HideInInspector] public TMPro.TextMeshProUGUI TimeText;
    [HideInInspector] public GameObject PopupWindowPrefab; // Pop-up window to show instructions and results
    [HideInInspector] public string DebugID = "[PuzzleManager]";
    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    [HideInInspector] public UIPaintingMiniGame MinigameCanvas;

    void Start(){
        MinigameCanvas = GameObject.Find("MainCanvas").GetComponent<UIPaintingMiniGame>();
        PuzzleTimer = GetComponent<Timer>();
        try{
            PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
            PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
                "Welkom bij de schilderij puzzel! Plaats de stukken op de juiste plek om het kunstwerk te voltooien.",
                "Sleep elk puzzelstuk onderin het scherm naar de juiste positie op het raster.",
                "Als alle stukken correct geplaatst zijn, voltooi je de puzzel. Hoe sneller je het voltooit, hoe meer punten je verdient. Veel succes!"
            };
            if (DebugMode){ Debug.Log($"{DebugID} Starting Puzzle Manager, showing instructions popup."); }
            GameObject popupInstance = Instantiate(PopupWindowPrefab, MinigameCanvas.transform);
            popupInstance.GetComponent<PopUpWindow>().OnPopUpClosed += StartPuzzle;
        }
        catch{ Debug.LogError("PuzzleManager: Could not load PopUpPanel prefab from Resources."); }
    }

    void Update(){
        if (PuzzleTimer.IsRunning()){
            UpdateTimeDisplay();
        }
    }

    public void UpdateTimeDisplay(){
        float elapsed = PuzzleTimer.GetElapsedTime();
        MinigameCanvas.SetTimeText(elapsed);
    }


    public void StartPuzzle(){
        if (DebugMode){ Debug.Log($"{DebugID} Starting puzzle timer."); }
        PuzzleTimer.StartTimer();
    }

    public void QuitPuzzle(){
        if (DebugMode){ Debug.Log($"{DebugID} Quitting puzzle and returning to main scene."); }
        PuzzleTimer.Stop();
        GameManager.Instance.LoadSchoolMap();
    }

    /// <summary>
    /// Stops the timer and processes the puzzle completion, awarding points to the player and showing results.
    /// </summary>
    public void StopPuzzle(){
        if (DebugMode){ Debug.Log($"{DebugID} All pieces correctly placed! Stopping puzzle."); }
        PuzzleTimer.Stop();
        float timeTaken = PuzzleTimer.GetElapsedTime();
        // TODO: Needs to be done from a database
        ActivityData activityData = ScriptableObject.CreateInstance<ActivityData>();
        activityData.ActivityName = this.Puzzlename;
        activityData.ActivityType = this.PuzzleType;
        activityData.ActivityDuration = timeTaken;
        activityData.ActivityPoints = CalculatePoints(timeTaken); // Example points
        activityData.RewardCoins = this.RewardCoins;

        // Create a PopUp that blocks the screen and shows the results
        if (DebugMode){ Debug.Log($"{DebugID} Creating new canvas"); }
        PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
        PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = new List<string>{
            $"Gefeliciteerd! Je hebt de puzzel in {timeTaken:F2} seconden voltooid.",
            $"Je hebt {activityData.ActivityPoints} punten en {activityData.RewardCoins} <Sprite index=0> verdiend voor je prestatie.\n\n Goed gedaan!"
        };
        GameObject popupInstance = Instantiate(PopupWindowPrefab, MinigameCanvas.transform);
        popupInstance.GetComponent<PopUpWindow>().OnPopUpClosed += QuitPuzzle;
    }

    /// <summary>
    /// 
    /// </summary>
    public int CalculatePoints(float timeTaken){
        return (int)(300f-timeTaken+5f);
    }

    
}
