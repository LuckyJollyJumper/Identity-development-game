using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI class that controls all the UI elements for the persona game
/// </summary>
public class UIPersonaGame : MonoBehaviour
{
    [Header("Settings")]
    // {SelectedPersonas1, SelectedPersonas2, SelectedPersonas3, SelectedPersonas4, SelectedPersonas5, SelectedPersonas6}
    [SerializeField] private string PersonaType = "SelectedPersonas1";
    [TextArea][SerializeField] private string QuestionText = "";
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI QuestionTextObject;
    [SerializeField] public GameObject Grid;

    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    private string DebugID;

    void Start(){
        this.DebugID = $"[UI{PersonaType}]";
        this.QuestionTextObject.text = QuestionText;
    }


    /// <summary>
    /// Called on the submit button and will return to the main schoolmap scene while saving the game
    /// </summary>
    public void OnSubmitPersona(){
        GameManager.Instance.SetPlayerData(PersonaType, string.Join(", ", GetAllGridItemSelections()));
        GameManager.Instance.LoadSchoolMap();
    }

    /// <summary>
    /// Returns all the Items that were selected by the player in Grid.
    /// </summary>
    /// <returns></returns>
    public string[] GetAllGridItemSelections(){
        if (Grid == null){ Grid = GameObject.Find("FirstPersonaTest"); }
        SelectedItem[] items = Grid.GetComponentsInChildren<SelectedItem>();

        var selectedStates = new List<string>();
        foreach(SelectedItem item in items){
            if (item.IsSelected){
                selectedStates.Add(item.DisplayText);
            }
        }
        if (DebugMode){Debug.Log($"{DebugID} Selected personas: " + string.Join(", ", selectedStates));}
        return selectedStates.ToArray();
    }
}
