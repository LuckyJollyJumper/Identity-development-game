using UnityEngine;
using System.Collections.Generic;

public class UIPaintingMiniGame : MonoBehaviour
{
    [HideInInspector] public TMPro.TextMeshProUGUI TimeText;
    public List<DropLocation> DropLocations;
    public List<PuzzlePieceItem> PuzzlePieces;
    public PuzzleManager PuzzleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        PuzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        
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
            if (piece.gridPosition != dropLocation.GridCoordinate){
                Debug.Log($"Piece at {dropLocation.GridCoordinate} is incorrect. piece at {piece.gridPosition} with drop at {dropLocation.GridCoordinate}"); 
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
