using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    public bool isSelected = false;
    public string category;

    public void Awake()
    {
        this.category = gameObject.GetComponentInChildren<TMPro.TMP_Text>().text;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        this.GetComponent<UnityEngine.UI.RawImage>().color = isSelected ? Color.green : Color.white;
    }
    
}
