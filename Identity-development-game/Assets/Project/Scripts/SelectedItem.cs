using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on " + gameObject.name);
        isSelected = !isSelected;
        // You can add visual feedback for selection here
        this.GetComponent<UnityEngine.UI.RawImage>().color = isSelected ? Color.green : Color.white;
    }
    
}
