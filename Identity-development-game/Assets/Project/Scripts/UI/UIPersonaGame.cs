using UnityEngine;
using System.Collections.Generic;

public class UIPersonaGame : MonoBehaviour
{
    [SerializeField] public GameObject grid;

    public void OnSubmitPersona(){
        GameManager.Instance.SetPlayerDataField("selectedPersonas1", string.Join(", ", GetAllGridItemSelections()));
        GameManager.Instance.SaveGame();
        GameManager.Instance.LoadScene(ScenesManager.scenes.MainMenu);
        if (grid == null){ grid = GameObject.Find("VerticalLayout"); }
    }

    public string[] GetAllGridItemSelections(){
        SelectedItem[] items = grid.GetComponentsInChildren<SelectedItem>();

        var selectedStates = new List<string>();
        foreach(SelectedItem item in items){
            if (item.isSelected){
                selectedStates.Add(item.category);
            }
        }
        Debug.Log("Selected personas: " + string.Join(", ", selectedStates));
        return selectedStates.ToArray();
    }
}
