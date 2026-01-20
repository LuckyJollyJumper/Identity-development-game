using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropLocation : MonoBehaviour
{
    [HideInInspector] public Vector2Int gridCoordinate;
    [HideInInspector] public GameObject placedItem;
    [SerializeField] bool DebugMode = false;
    private string DebugID;

    public void Start(){
        DebugID = $"[DropLocation {gameObject.name}]";
        Vector2Int coord = GetGridCoordinate();
    }

    /// <summary>
    /// Removes the reference to the placed item. Used by items that are moved off this drop location.
    /// </summary>
    public void RemovePlacedItem(){ placedItem = null; }


    /// <summary>
    /// Calculates the (x, y) grid coordinate of this drop location.
    /// </summary>
    public Vector2Int GetGridCoordinate(){
        int index = transform.GetSiblingIndex();
        
        GridLayoutGroup gridLayout = transform.parent.GetComponent<GridLayoutGroup>();
        if (gridLayout != null){
            int columns = Mathf.Max(1, gridLayout.constraintCount);
            int x = index % columns;
            int y = index / columns;
            return new Vector2Int(x, y);
        }
        // Fallback: return index as x coordinate
        return new Vector2Int(index, 0);
    }
}
