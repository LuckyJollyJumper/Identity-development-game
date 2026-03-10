using UnityEngine;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Very ugly fix to add a choice between 2 options in the characters
/// </summary>
public class ChoiceField : MonoBehaviour
{
    [HideInInspector] public GameObject parent;
    [SerializeField] private TextMeshProUGUI Button1;
    [SerializeField] private TextMeshProUGUI Button2;
    [SerializeField] private TextMeshProUGUI PopUpText;
    public void SetChoices(List<string> buttons, GameObject parent){
        this.PopUpText.text = buttons[0];
        this.Button1.text = buttons[1];
        this.Button2.text = buttons[2];
        this.parent = parent;
    }
    public void ButtonPressed(){
        parent.GetComponent<PopUpWindow>().EndInteraction();
    }
}
