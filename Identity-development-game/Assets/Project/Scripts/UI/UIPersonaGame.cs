using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI class that controls all the UI elements for the persona game
/// </summary>
public class UIPersonaGame : MonoBehaviour
{
    [SerializeField] public GameObject Grid;

    public void OnSubmitPersona(){
        GameManager.Instance.SetPlayerDataField("selectedPersonas1", string.Join(", ", GetAllGridItemSelections()));
        GameManager.Instance.SaveGame();
        GameManager.Instance.LoadScene(ScenesManager.Scenes.SchoolMap);
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
