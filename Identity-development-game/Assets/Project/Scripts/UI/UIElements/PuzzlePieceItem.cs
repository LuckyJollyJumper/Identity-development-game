using UnityEngine;

public class PuzzlePieceItem : DraggableItem
{
    [Header("Puzzle Piece Settings")]
    [Tooltip("The grid position this puzzle piece belongs to. Should match the DropLocation's grid coordinates.")]
    [SerializeField] public Vector2Int gridPosition; // Position in the puzzle grid

    public override void Start(){
        base.Start();
        if (DebugMode){
            this.GetComponentInChildren<TMPro.TMP_Text>().text = $"({gridPosition.x}, {gridPosition.y})";
        }else{
            this.GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }
    }

}
