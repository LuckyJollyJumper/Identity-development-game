using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropLocation : MonoBehaviour
{
    [SerializeField] public Vector2Int GridCoordinate;
    [SerializeField] public GameObject PlacedItem;

    public void Start(){
        GridCoordinate = GetGridCoordinate();
    }

    /// <summary>
    /// Removes the reference to the placed item. Used by items that are moved off this drop location.
    /// </summary>
    public void ClearPlacedItem(){ PlacedItem = null; }
    public void SetPlacedItem(GameObject item){ 
        Debug.Log($"Setting placed item at {GridCoordinate} to {item.name}");
        PlacedItem = item; }


    /// <summary>
    /// Calculates the (x, y) grid coordinate of this drop location. 
    /// Uses GridLayoutGroup Constraint- `Fixed Column Count` to determine columns.
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
