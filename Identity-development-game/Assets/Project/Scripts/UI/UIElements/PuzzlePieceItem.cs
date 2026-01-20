using UnityEngine;

public class PuzzlePieceItem : DraggableItem
{
    [SerializeField] public Vector2Int gridPosition; // Position in the puzzle grid

    public override void Start(){
        this.GetComponentInChildren<TMPro.TMP_Text>().text = $"({gridPosition.x}, {gridPosition.y})";
        base.Start();
    }

}
