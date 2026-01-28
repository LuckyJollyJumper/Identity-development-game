using UnityEngine;

/// <summary>
/// An interactable object that starts an activity when interacted with.
/// </summary>
public class InteractableActivityObject : InteractableObject
{
    [Tooltip("The scene that the Object will launch when interacted with")]
    public ScenesManager.Scenes ActivityScene;
   
    public override void Start(){
        base.Start();
        this.UI.GetComponentInChildren<PopUpWindow>().OnPopUpClosed += StartActivity;
        if (DebugMode){Debug.Log($"{base.DebugID} {this.UI.GetComponentInChildren<PopUpWindow>().PopUpTexts.Count} pop-up texts set for activity object.");}
    }

    public override void OnEndInteract(){
        base.OnEndInteract();
        this.UI.GetComponent<PopUpWindow>().OnPopUpClosed -= StartActivity;
    }

    public void StartActivity(){
        if (DebugMode) {
            Debug.Log($"[DebugMode] Starting activity: {ActivityScene}");
        }
        GameManager.Instance._scenesManager.LoadScene(ActivityScene);
    }

}
