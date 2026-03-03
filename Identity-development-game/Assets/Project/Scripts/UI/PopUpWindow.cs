using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// A pop-up window that displays text and can cycle through multiple texts. It will pause player interaction while active.
/// Subscribing to OnPopUpClosed allows other scripts to trigger actions when the pop-up is closed. Make sure to set the PopUpTexts
/// before it gets the Start method is called.
/// </summary>
public class PopUpWindow : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("List of texts to display in the pop-up window, in order")]
    [TextArea][SerializeField] public List<string> PopUpTexts;
    [SerializeField] public List<AudioClip> PopUpAudioClips; // Optional audio clips for each pop-up text
    [SerializeField] public List<ChoiceField> PopUpChoiceOptions;
    [Tooltip("If true, the pop-up delete itself after all texts are displayed.")]
    [SerializeField] public bool DeleteOnClose = true; // If true, the pop-up GameObject will be destroyed when the interaction ends. Otherwise, it will just be deactivated.
    [Header("Debug")]
    [SerializeField] protected bool DebugMode = false;
    protected string DebugID = "[PopUpWindow]";
    protected Playercontroller Player;
    protected TextMeshProUGUI PopUpText;
    protected int CurrentTextIndex = 0;
    [HideInInspector] public event Action OnPopUpClosed; // Event triggered when the pop-up is closed

    public virtual void Start(){
        this.PopUpText = GameObject.Find("PopUpText").GetComponent<TMPro.TextMeshProUGUI>();
        this.Player = FindFirstObjectByType<Playercontroller>();
        if (this.Player != null) {
            this.Player.StartInteraction();
        }
        NextPopUpText();
    }


    /// <summary>
    /// Displays the next text in the pop-up sequence or closes the pop-up if there are no more texts using currentTextIndex.
    /// </summary>
    public virtual void NextPopUpText(){
        if (this.CurrentTextIndex < this.PopUpTexts.Count){
            SetPopUpText(PopUpTexts[this.CurrentTextIndex]);
            PlayCurrenAudioClip();
        }else{
            if (DebugMode){ Debug.Log($"{DebugID} No more pop-up texts to display"); }
            EndInteraction();
        }
        this.CurrentTextIndex++;
    }
    protected virtual void SetPopUpText(string text){ 
        if (PopUpText == null){ 
            if (DebugMode){Debug.Log($"{DebugID} PopUpText is apparently empty");}
            this.PopUpText = GameObject.Find("PopUpText").GetComponent<TMPro.TextMeshProUGUI>(); 
        }
        this.PopUpText.text = text; 
    }
    /// <summary>
    /// Syncs the audio clips from the PopUpAudioClips list with the current text index and plays the corresponding audio clip if it exists.
    /// </summary>
    protected void PlayCurrenAudioClip(){
        if (PopUpAudioClips.Count > this.CurrentTextIndex){
            if(PopUpAudioClips[this.CurrentTextIndex] != null){
                if (DebugMode){ Debug.Log($"{DebugID} Playing audio clip for pop-up text index {this.CurrentTextIndex}"); }
                SoundManager.Instance.PlaySound(PopUpAudioClips[this.CurrentTextIndex]);
            }
        }
    }


    /// <summary>
    /// Ends the pop-up interaction, invokes the OnPopUpClosed event, deactivates 
    /// the pop-up window and allows the player to resume interaction.
    /// </summary>
    public virtual void EndInteraction(){
        if (this.Player != null){
            this.Player.EndInteraction();
        }

        OnPopUpClosed?.Invoke();
        this.gameObject.SetActive(false);
        
        if (DebugMode){ Debug.Log($"{DebugID} Pop-up interaction ended and window closed"); }
        if (DeleteOnClose) { Destroy(this.gameObject); }
    }


    /// <summary>
    /// Used to detect clicks on the pop-up window. When the pop-up is clicked, it will call NextPopUpText() to 
    /// either show the next text or close the pop-up if there are no more texts.
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData){
        NextPopUpText();
        if (DebugMode){ Debug.Log($"{DebugID} PopUpWindow clicked"); }
    }

    /// <summary>
    /// Used by other scripts to clear all subscribed actions to the OnPopUpClosed event.
    /// Used to fully reset the PopUpWindow. Used by the quests characters.
    /// </summary>
    public virtual void ClearActions(){
        OnPopUpClosed = null;
        if (DebugMode){ Debug.Log($"{DebugID} OnPopUpClosed event cleared"); }
    }

}
