using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// A pop-up window that displays text and can cycle through multiple texts. It will pause player interaction while active.
/// </summary>
public class PopUpWindow : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("List of texts to display in the pop-up window, in order")]
    [SerializeField] public List<string> popUpTexts;
    [Header("Debug")]
    [SerializeField] public bool debugMode = false;
    protected string DebugID = "[PopUpWindow]";
    protected Playercontroller player;
    protected TextMeshProUGUI popUpText;
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


    /// <summary>
    /// Displays the next text in the pop-up sequence or closes the pop-up if there are no more texts using currentTextIndex.
    /// </summary>
    public void NextPopUpText(){
        if (currentTextIndex < popUpTexts.Count){
            SetPopUpText(popUpTexts[currentTextIndex]);
        }else{
            if (debugMode){ Debug.Log($"{DebugID} No more pop-up texts to display"); }
            EndInteraction();
        }
        this.currentTextIndex++;
    }
    public void SetPopUpText(string text){ this.popUpText.text = text; }


    /// <summary>
    /// Ends the pop-up interaction, invokes the OnPopUpClosed event, deactivates 
    /// the pop-up window and allows the player to resume interaction.
    /// </summary>
    public virtual void EndInteraction(){
        if (this.player != null){
            this.player.EndInteraction();
        }

        OnPopUpClosed?.Invoke();
        this.gameObject.SetActive(false);
        
        if (debugMode){ Debug.Log($"{DebugID} Pop-up interaction ended and window closed"); }
        Destroy(this.gameObject);
       
    }


    public virtual void OnPointerClick(PointerEventData eventData){
        NextPopUpText();
        if (debugMode){ Debug.Log($"{DebugID} PopUpWindow clicked"); }
    }

}
