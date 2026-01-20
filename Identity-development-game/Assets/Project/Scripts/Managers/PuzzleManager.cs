using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public List<PuzzleDropLocation> dropLocations;
    public Timer puzzleTimer;

    public void Start(){
        dropLocations = new List<PuzzleDropLocation>(FindObjectsOfType<PuzzleDropLocation>());
    }

    /// <summary>
    ///  Checks if all puzzle pieces are correctly placed.
    /// </summary>
    public bool CheckCorrectPlacements(){
         bool isCorrect = true;
        foreach (PuzzleDropLocation dropLocation in dropLocations){
            isCorrect &= dropLocation.placedItem.GetComponent<PuzzlePieceItem>().gridPosition == dropLocation.gridCoordinate;
        }
        return isCorrect;
    }
}
