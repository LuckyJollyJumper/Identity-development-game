using UnityEngine;

public class PuzzlePieceItem : DraggableItem
{
    [Header("Puzzle Piece Settings")]
    [Tooltip("The grid position this puzzle piece belongs to. Should match the DropLocation's grid coordinates.")]
    [SerializeField] public Vector2Int GridPosition; // Position in the puzzle grid

    public override void Start(){
        base.Start();
        if (DebugMode){
            this.GetComponentInChildren<TMPro.TMP_Text>().text = $"({GridPosition.x}, {GridPosition.y})";
        }else{
            this.GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }
    }

}
