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
    [TextArea][SerializeField] public List<string> PopUpTexts;
    [SerializeField] public List<AudioClip> PopUpAudioClips; // Optional audio clips for each pop-up text
    [Tooltip("If true, the pop-up delete itself after all texts are displayed.")]
    [SerializeField] public bool DeleteOnClose = true; // If true, the pop-up GameObject will be destroyed when the interaction ends. Otherwise, it will just be deactivated.
    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
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
            if (PopUpAudioClips.Count >= this.CurrentTextIndex && PopUpAudioClips[this.CurrentTextIndex] != null){
                SoundManager.Instance.PlaySound(PopUpAudioClips[this.CurrentTextIndex]); // Play corresponding audio clip if available
            }
        }else{
            if (DebugMode){ Debug.Log($"{DebugID} No more pop-up texts to display"); }
            EndInteraction();
        }
        this.CurrentTextIndex++;
    }
    public virtual void SetPopUpText(string text){ 
        if (PopUpText == null){ 
            if (DebugMode){Debug.Log($"{DebugID} PopUpText is apparently empty");}
            this.PopUpText = GameObject.Find("PopUpText").GetComponent<TMPro.TextMeshProUGUI>(); 
        }
        this.PopUpText.text = text; 
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


    public virtual void OnPointerClick(PointerEventData eventData){
        NextPopUpText();
        if (DebugMode){ Debug.Log($"{DebugID} PopUpWindow clicked"); }
    }

    public virtual void ClearActions(){
        OnPopUpClosed = null;
        if (DebugMode){ Debug.Log($"{DebugID} OnPopUpClosed event cleared"); }
    }

}
