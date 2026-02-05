using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manager class to handle the puzzle game from start to finish.
/// </summary>
public class PuzzleActivityManager : ActivityManager
{
    public override void Start(){
        base.Start();
        base.ActivityCanvas = GameObject.Find("MainCanvas");
        List<string> popUpTexts = new List<string>{
                "Welkom bij de schilderij puzzel! Plaats de stukken op de juiste plek om het kunstwerk te voltooien.",
                "Sleep elk puzzelstuk onderin het scherm naar de juiste positie op het raster.",
                "Als alle stukken correct geplaatst zijn, voltooi je de puzzel. Hoe sneller je het voltooit, hoe meer punten je verdient. Veel succes!"
        };
        base.CreatePopUp(popUpTexts).GetComponent<PopUpWindow>().OnPopUpClosed += StartActivity;
    }


    /// <summary>
    /// Stops the timer and processes the puzzle completion, awarding points to the player and showing results.
    /// This function is called from the ActivityCanvas (UIPaintingMiniGame)
    /// </summary>
    public void StopPuzzle(){
        if (DebugMode){ Debug.Log($"{DebugID} All pieces correctly placed! Stopping puzzle."); }
        ActivityTimer.Stop();
        float timeTaken = ActivityTimer.GetElapsedTime();

        base.SaveActivityData();

        // Create a PopUp that blocks the screen and shows the results
        List<string> popUpTexts = new List<string>{
            $"Gefeliciteerd! Je hebt de puzzel in {timeTaken:F2} seconden voltooid.",
            $"Je hebt {CalculatePoints(timeTaken)} punten en {RewardCoins} <Sprite index=0> verdiend voor je prestatie.\n\n Goed gedaan!"
        };
        base.CreatePopUp(popUpTexts).GetComponent<PopUpWindow>().OnPopUpClosed += QuitActivity;
    }

    public override void UpdateTimeDisplay(){
        float elapsed = ActivityTimer.GetElapsedTime();
        ActivityCanvas.GetComponent<UIPaintingMiniGame>().SetTimeText(elapsed);
    }

    

    
}
