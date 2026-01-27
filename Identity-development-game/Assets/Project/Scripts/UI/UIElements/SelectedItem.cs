using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Parent class of a UI element that can be toggled on and off. Used for the persona questions.
/// </summary>
public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public bool IsSelected = false;
    [Tooltip("Text to be displayed for this item")]
    [SerializeField] public string DisplayText;
    [Tooltip("Image to explain the activity visually")]
    [SerializeField] public Sprite ExampleImage;

    protected Image Background;
    protected Image ExampleImageObject;

    public virtual void Awake(){
        this.Background = this.transform.Find("BackgroundImage").GetComponent<Image>();

        gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = DisplayText;
        if(ExampleImage != null){
            this.ExampleImageObject = this.transform.Find("ItemImage").GetComponent<Image>();
            this.ExampleImageObject.sprite = this.ExampleImage;
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData){
        IsSelected = !IsSelected;
        this.Background.color = IsSelected ? Color.green : Color.white;
    }
    
}


