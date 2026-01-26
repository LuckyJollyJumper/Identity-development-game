using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI class that controls all the UI elements for the puzzle minigame.
/// </summary>
public class UIPaintingMiniGame : MonoBehaviour
{
    [HideInInspector] public TMPro.TextMeshProUGUI TimeText;
    public List<DropLocation> DropLocations;
    public List<PuzzlePieceItem> PuzzlePieces;
    public PuzzleManager PuzzleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        PuzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        if (PuzzleManager == null)
        {
            Debug.Log($"[UIPaintingMiniGame] No PuzzleManager found");
        }
        
        DropLocations = new List<DropLocation>(FindObjectsByType<DropLocation>(FindObjectsSortMode.None));
        DropLocation startDropLocation = DropLocations.Find(dl => dl.gameObject.name == "StartDropLocation");
        if (startDropLocation != null){
            DropLocations.Remove(startDropLocation);
        }

        PuzzlePieces = new List<PuzzlePieceItem>(FindObjectsByType<PuzzlePieceItem>(FindObjectsSortMode.None));
        foreach (PuzzlePieceItem piece in PuzzlePieces){
            piece.OnItemPlaced += CheckPuzzleCompletion;
        }

        TimeText = transform.Find("TimerText").GetComponent<TMPro.TextMeshProUGUI>();
        TimeText.text = "Tijd: 0.00 s";
    }

    public void CheckPuzzleCompletion(){
        Debug.Log("Checking puzzle completion...");
        if (AllPiecesCorrect()){
            Debug.Log("Puzzle completed.");
            PuzzleManager.StopPuzzle();
        }
        
    }

   
    /// <summary>
    ///  Checks if all puzzle pieces are correctly placed in the grid.
    /// </summary>
    public bool AllPiecesCorrect(){
        bool isCorrect = true;
        foreach (DropLocation dropLocation in DropLocations){
            if (dropLocation.PlacedItem == null){
                Debug.Log($"Piece at {dropLocation.GridCoordinate} is missing.");
                return isCorrect = false;
            }
            PuzzlePieceItem piece = dropLocation.PlacedItem.GetComponent<PuzzlePieceItem>();
            if (piece.GridPosition != dropLocation.GridCoordinate){
                Debug.Log($"Piece at {dropLocation.GridCoordinate} is incorrect. piece at {piece.GridPosition} with drop at {dropLocation.GridCoordinate}"); 
                return isCorrect = false;
            }
            Debug.Log($"Piece at {dropLocation.GridCoordinate} is correct.");
        }
        return isCorrect;
    }

    public void SetTimeText(float elapsed){
        TimeText.text = $"Tijd: {elapsed:F2} s";
    }
}
