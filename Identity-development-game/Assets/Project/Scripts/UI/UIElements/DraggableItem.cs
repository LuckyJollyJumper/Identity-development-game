using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    public Transform ParentAfterDrag;
    private Image ItemImage;
    private Transform CanvasObject;

    public virtual void Start(){
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
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform as RectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out globalMousePos)){ transform.position = globalMousePos; }
    }

    public void OnEndDrag(PointerEventData eventData){
        GameObject lowerObject = eventData.pointerCurrentRaycast.gameObject;
        if (lowerObject != null){
            DropLocation dropLocation = lowerObject.GetComponent<DropLocation>();
            if (dropLocation != null){
                // Check for empty drop location
                if (dropLocation.placedItem == null){
                    Debug.Log("Valid location: " + lowerObject.name);
                    dropLocation.placedItem = this.gameObject;
                    this.ParentAfterDrag = dropLocation.transform;  // Set new parent

                    if (lowerObject != ParentAfterDrag){
                        // If the item was dropped onto a different location,
                        // inform that location to remove its reference
                        ParentAfterDrag.GetComponent<DropLocation>().RemovePlacedItem();
                    }
                }else{
                    Debug.Log("Drop location occupied: " + lowerObject.name);
                }
            }
        }
        Debug.Log("Ending drag, going to: " + ParentAfterDrag.name);
        transform.SetParent(ParentAfterDrag);
        this.ItemImage.raycastTarget = true;
    }

}
