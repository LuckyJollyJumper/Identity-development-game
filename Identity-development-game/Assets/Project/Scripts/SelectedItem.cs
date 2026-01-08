using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Image to explain the activity visually")]

    [Header("Selection State")]
    [SerializeField] public bool isSelected = false;
    [Tooltip("Text to be displayed for this item")]
    [SerializeField] public string displayText;
    [SerializeField] public UnityEngine.UI.RawImage exampleImage;

    
    [Header("References")]
    [SerializeField] public UnityEngine.UI.Image background;
    [SerializeField] public UnityEngine.UI.RawImage exampleImagePlaceHolder;

    public virtual void Awake()
    {
        gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = displayText;
        try{this.exampleImagePlaceHolder = this.exampleImage;}catch{}
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        this.background.color = isSelected ? Color.green : Color.white;
    }
    
}


