using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    public event Action OnItemPlaced; // Event triggered when the item is placed on a new empty drop location
    [Tooltip("Parent transform to return to after dragging")]
    public Transform ParentAfterDrag;
    private Image ItemImage;
    private Transform CanvasObject;
    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    private string DebugID;
    

    public virtual void Start(){
        DebugID = $"[DraggableItem// {gameObject.name}]";
        CanvasObject = GameObject.Find("MainCanvas").transform;
        ParentAfterDrag = this.transform.parent;
        ItemImage = this.GetComponentInChildren<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        transform.SetParent(CanvasObject); // Move to top level to avoid being clipped by other UI elements
        transform.SetAsLastSibling(); // Ensure it's on top of other siblings
        ItemImage.raycastTarget = false; // Disable raycast target so it doesn't block other UI elements
    }

    public void OnDrag(PointerEventData eventData){
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 globalMousePos)) { transform.position = globalMousePos; }
    }

    public void OnEndDrag(PointerEventData eventData){
        GameObject lowerObject = eventData.pointerCurrentRaycast.gameObject;
        if (lowerObject != null){
            DropLocation dropLocation = lowerObject.GetComponent<DropLocation>();
            if (dropLocation != null){
                // Check for empty drop location
                if (dropLocation.PlacedItem == null){
                    if (DebugMode){ Debug.Log($"{DebugID} Found empty drop location: {lowerObject.name}"); }
                    
                    // If the item was previously placed on another drop location, clear that reference
                    DropLocation oldDropLocation = ParentAfterDrag.GetComponent<DropLocation>();
                    if (oldDropLocation != null){
                        oldDropLocation.ClearPlacedItem();
                    }
                    
                    dropLocation.SetPlacedItem(this.gameObject);
                    this.ParentAfterDrag = dropLocation.transform;  // Set new parent
                    OnItemPlaced?.Invoke();
                }else{
                    if (DebugMode){ Debug.Log($"{DebugID} Drop location occupied: {lowerObject.name}"); }
                }
            }
        }
        if (DebugMode){ Debug.Log($"{DebugID}Ending drag, going to: {ParentAfterDrag.name}"); }
        transform.SetParent(ParentAfterDrag);
        this.ItemImage.raycastTarget = true;
    }

}
