using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI class that controls all the UI elements for the persona game
/// </summary>
public class UIPersonaGame : MonoBehaviour
{
    public enum PersonaType {SelectedPersonas1, SelectedPersonas2, SelectedPersonas3, SelectedPersonas4, SelectedPersonas5, SelectedPersonas6};
    [SerializeField] public GameObject Grid;

    /// <summary>
    /// Called on the submit button and will return to the main schoolmap scene
    /// </summary>
    public void OnSubmitPersona(){
        GameManager.Instance.SetPlayerDataField("SelectedPersonas1", string.Join(", ", GetAllGridItemSelections()));
        GameManager.Instance.SaveGame();
        GameManager.Instance.LoadSchoolMap();
    }

    public string[] GetAllGridItemSelections(){
        if (Grid == null){ Grid = GameObject.Find("FirstPersonaTest"); }
        SelectedItem[] items = Grid.GetComponentsInChildren<SelectedItem>();

        var selectedStates = new List<string>();
        foreach(SelectedItem item in items){
            if (item.IsSelected){
                selectedStates.Add(item.DisplayText);
            }
        }
        Debug.Log("Selected personas: " + string.Join(", ", selectedStates));
        return selectedStates.ToArray();
    }
}
