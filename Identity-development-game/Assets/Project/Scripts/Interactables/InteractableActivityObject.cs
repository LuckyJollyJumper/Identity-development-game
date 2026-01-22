using UnityEngine;

/// <summary>
/// An interactable object that starts an activity when interacted with.
/// </summary>
public class InteractableActivityObject : InteractableObject
{
    public ScenesManager.scenes activityScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start(){
        base.Start();
        this.UI.GetComponent<PopUpWindow>().OnPopUpClosed += StartActivity;
    }

    public override void OnEndInteract(){
        base.OnEndInteract();
        this.UI.GetComponent<PopUpWindow>().OnPopUpClosed -= StartActivity;
    }

    public void StartActivity(){
        if (DebugMode) {
            Debug.Log($"[DebugMode] Starting activity: {activityScene}");
        }
        GameManager.Instance._scenesManager.LoadScene(activityScene);
    }

}
