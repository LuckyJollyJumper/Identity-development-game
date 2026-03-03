using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI class that controls all the UI elements for the puzzle minigame.
/// </summary>
public class UIClothingMiniGame : MonoBehaviour
{
    public List<DropLocation> DropLocations;
    public List<ClothingPieceItem> ClothingPieces;
    public PuzzleActivityManager PuzzleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

    }

    public void CheckPuzzleCompletion(){
        // Debug.Log("Checking puzzle completion...");
        // if (AllPiecesCorrect()){
        //     Debug.Log("Puzzle completed.");
        //     PuzzleManager.StopPuzzle();
        // }
        
    }

   
    /// <summary>
    ///  Checks if all puzzle pieces are correctly placed in the grid.
    /// </summary>
    // public bool AllPiecesCorrect(){
    //     bool isCorrect = true;
    //     foreach (DropLocation dropLocation in DropLocations){
    //         if (dropLocation.PlacedItem == null){
    //             Debug.Log($"Piece at {dropLocation.GridCoordinate} is missing.");
    //             return isCorrect = false;
    //         }
    //         PuzzlePieceItem piece = dropLocation.PlacedItem.GetComponent<PuzzlePieceItem>();
    //         if (piece.GridPosition != dropLocation.GridCoordinate){
    //             Debug.Log($"Piece at {dropLocation.GridCoordinate} is incorrect. piece at {piece.GridPosition} with drop at {dropLocation.GridCoordinate}"); 
    //             return isCorrect = false;
    //         }
    //         Debug.Log($"Piece at {dropLocation.GridCoordinate} is correct.");
    //     }
    //     return isCorrect;
    // }

}
