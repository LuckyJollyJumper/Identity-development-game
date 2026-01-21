using UnityEngine;
using System;
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
    [Header("Debug")]
    [SerializeField] public bool debugMode = false;
    protected Playercontroller player;
    protected TextMeshProUGUI popUpText;
    protected string DebugID = "[PopUpWindow]";
    protected int currentTextIndex = 0;
    [HideInInspector] public event Action OnPopUpClosed; // Event triggered when the pop-up is closed

    public virtual void Start(){
        this.popUpText = GameObject.Find("PopUpText").GetComponent<TMPro.TextMeshProUGUI>();
        this.player = FindFirstObjectByType<Playercontroller>();
        if (player != null) {
            this.player.StartInteraction();
        }
       
        NextPopUpText();
    }

    public void SetPopUpText(string text){
        this.popUpText.text = text;
    }

    public void NextPopUpText(){
        if (currentTextIndex < popUpTexts.Count){
            SetPopUpText(popUpTexts[currentTextIndex]);
        }else{
            if (debugMode){ Debug.Log($"{DebugID} No more pop-up texts to display"); }
            EndInteraction();
        }
        this.currentTextIndex++;
    }

    public virtual void EndInteraction(){
        if (this.player != null){
            this.player.EndInteraction();
        }

        OnPopUpClosed?.Invoke();
        this.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData){
        NextPopUpText();
        if (debugMode){ Debug.Log($"{DebugID} PopUpWindow clicked"); }
    }

}
