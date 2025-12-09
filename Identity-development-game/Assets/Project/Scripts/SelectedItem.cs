using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        this.GetComponent<UnityEngine.UI.RawImage>().color = isSelected ? Color.green : Color.white;
    }
    
}
