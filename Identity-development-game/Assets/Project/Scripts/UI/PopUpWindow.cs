using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// A pop-up window that displays text and can cycle through multiple texts.
/// </summary>
public class PopUpWindow : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("List of texts to display in the pop-up window, in order")]
    [SerializeField] public List<string> popUpTexts;
    protected Playercontroller player;
    protected TextMeshProUGUI popUpText;
    protected string DebugID = "[PopUpWindow]";
    protected int currentTextIndex = 0;

    public virtual void Start(){
        player = FindFirstObjectByType<Playercontroller>();
        popUpText = this.GetComponentInChildren<TextMeshProUGUI>();
        player.StartInteraction();

        NextPopUpText();
    }

    public void SetPopUpText(string text){
        popUpText.text = text;
    }

    public void NextPopUpText(){
        if (currentTextIndex < popUpTexts.Count){
            SetPopUpText(popUpTexts[currentTextIndex]);
        }else{
            Debug.Log($"{DebugID} No more pop-up texts to display");
            EndInteraction();
        }
        currentTextIndex++;
    }

    public virtual void EndInteraction(){
        player.EndInteraction();
        this.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData){
        NextPopUpText();
        Debug.Log($"{DebugID} PopUpWindow clicked");
    }

}
