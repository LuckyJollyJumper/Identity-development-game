using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Image to explain the activity visually")]

    [Header("Selection State")]
    [SerializeField] public bool IsSelected = false;
    [Tooltip("Text to be displayed for this item")]
    [SerializeField] public string DisplayText;
    [SerializeField] public UnityEngine.UI.RawImage ExampleImage;

    
    [Header("References")]
    [SerializeField] public UnityEngine.UI.Image Background;
    [SerializeField] public UnityEngine.UI.RawImage ExampleImagePlaceHolder;

    public virtual void Awake()
    {
        gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = DisplayText;
        try{this.ExampleImagePlaceHolder = this.ExampleImage;}catch{}
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        IsSelected = !IsSelected;
        this.Background.color = IsSelected ? Color.green : Color.white;
    }
    
}


