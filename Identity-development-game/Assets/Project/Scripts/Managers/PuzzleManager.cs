using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manager class to handle the puzzle game from start to finish.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
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
            PopupWindowPrefab.GetComponent<PopUpWindow>().popUpTexts = new List<string>{
                "Welkom bij het schilderij puzzel! Plaats de stukken op de juiste plek om het kunstwerk te voltooien.",
                "Sleep elk puzzelstuk onderin het scherm naar de juiste positie op het raster.",
                "Als alle stukken correct geplaatst zijn, voltooi je de puzzel. Hoe sneller je het voltooit, hoe meer punten verdien je. Veel succes!"
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

    /// <summary>
    /// Stops the timer and processes the puzzle completion, awarding points to the player and showing results.
    /// </summary>
    public void StopPuzzle(){
        if (DebugMode){ Debug.Log($"{DebugID} All pieces correctly placed! Stopping puzzle."); }
        PuzzleTimer.Stop();
        float timeTaken = PuzzleTimer.GetElapsedTime();
        // TODO: Needs to be done from a database
        ActivityData activityData = new(){
            activityName = "Art Puzzle",
            activityType = ActivityData.ActivityType.Art,
            activityDuration = timeTaken,
            activityPoints = (int)(400f-timeTaken), // Example points
            rewardCoins = 1
        };

        // Create a PopUp that blocks the screen and shows the results
        PopupWindowPrefab.GetComponent<PopUpWindow>().popUpTexts = new List<string>{
            $"Gefeliciteerd! Je hebt de puzzel in {timeTaken:F2} seconden voltooid.",
            $"Je hebt {activityData.activityPoints} punten en {activityData.rewardCoins} munt(en) verdiend voor je prestatie.\n\n Goed gedaan!"
        };
        Instantiate(PopupWindowPrefab, MinigameCanvas.transform);
    }

    
}
