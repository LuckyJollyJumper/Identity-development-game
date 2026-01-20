using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform ParentAfterDrag;
    public Image itemImage;

    public virtual void Start(){
        ParentAfterDrag = this.transform.parent;
        if (itemImage == null){
            itemImage = GetComponent<Image>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        transform.SetParent(transform.root); // Move to top level to avoid being clipped by other UI elements
        transform.SetAsLastSibling(); // Ensure it's on top of other siblings
        itemImage.raycastTarget = false; // Disable raycast target so it doesn't block other UI elements
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
        // transform.SetParent(ParentAfterDrag);
        itemImage.raycastTarget = true;
    }
}
