using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    [Header("Selection State")]
    [SerializeField] public bool isSelected = false;
    [SerializeField] public string category;
    [Header("References")]
    [SerializeField] public UnityEngine.UI.Image image;

    public virtual void Awake()
    {
        this.category = gameObject.GetComponentInChildren<TMPro.TMP_Text>().text;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        this.image.color = isSelected ? Color.green : Color.white;
    }
    
}


