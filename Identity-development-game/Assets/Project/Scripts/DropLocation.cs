using UnityEngine;
using UnityEngine.EventSystems;

public class DropLocation : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData){
        print("Dropped on " + this.name);
        GameObject droppedObject = eventData.pointerDrag;
        DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();
        draggableItem.ParentAfterDrag = this.transform;
    }
}
