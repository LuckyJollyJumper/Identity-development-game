using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Parent class of all activities. Has methods to quit and return to the main scene
/// </summary>
public class ActivityManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int ActivityID; // Unique identifier for the activity
    [SerializeField] public string ActivityName;
    [SerializeField] public ActivityData.ActivitiesType ActivityType;
    [SerializeField] public int RewardCoins;
    [HideInInspector] public Timer ActivityTimer;
    [HideInInspector] public TMPro.TextMeshProUGUI TimeText;
    [HideInInspector] public GameObject PopupWindowPrefab; // Pop-up window to show instructions and results
    [SerializeField] public GameObject ActivityCanvas;
    [Header("Debug")]
    [HideInInspector] public string DebugID = "[ActivityManager]";
    [SerializeField] public bool DebugMode = false;

    public virtual void Start(){
        ActivityTimer = GetComponent<Timer>();
    }

    public virtual void Update(){
        if (ActivityTimer.IsRunning()){
            UpdateTimeDisplay();
        }
    }
    public virtual void UpdateTimeDisplay(){
        float elapsed = ActivityTimer.GetElapsedTime();
    }

    /// <summary>
    /// Creates a PopUp, users of this function still need to subscribe to OnPopUpClosed() themselves
    /// </summary>
    public virtual GameObject CreatePopUp(List<string> popUpTexts){
        try{
            PopupWindowPrefab = Resources.Load<GameObject>("PopUpPanel");
            PopupWindowPrefab.GetComponent<PopUpWindow>().PopUpTexts = popUpTexts;

            GameObject popupInstance = Instantiate(PopupWindowPrefab, ActivityCanvas.transform);
            if (DebugMode){ Debug.Log($"{DebugID} showing popup."); }
            return popupInstance;
        }
        catch(System.Exception e){ 
            Debug.LogError($"{DebugID}: Could not load PopUpPanel prefab from Resources. Exception: {e.Message}"); 
            return null;
        }
    }

    /// <summary>
    /// Saves the activity data to the player via the Gamemanager. Should be called when the activity is completed.
    /// </summary>
    public virtual void SaveActivityData(){
         float timeTaken = ActivityTimer.GetElapsedTime();
        // TODO: Needs to be done from a database
        ActivityData activityData = new(){
            ActivityID = this.ActivityID,
            ActivityName = this.ActivityName,
            ActivityType = this.ActivityType,
            ActivityDuration = timeTaken,
            ActivityPoints = CalculatePoints(timeTaken), // Example points
            RewardCoins = this.RewardCoins,
            CompletionDate = new Date(),
        };

        GameManager.Instance.AddActivityData(activityData);
    }

    /// <summary>
    /// Maximum points is 300 and the first 5 seconds are not counted
    /// </summary>
    public virtual int CalculatePoints(float timeTaken){
        return (int)(300f-timeTaken+5f);
    }

    /// <summary>
    /// Used to start the timer and start the activity
    /// </summary>
    public virtual void StartActivity(){
        if (DebugMode){ Debug.Log($"{DebugID} Starting timer."); }
        ActivityTimer.StartTimer();
    }

    /// <summary>
    /// Used to return to the main scene. Does not save activityData
    /// </summary>
    public void QuitActivity(){
        if (DebugMode){ Debug.Log($"{DebugID} Returning to main scene."); }
        ActivityTimer.Stop();
        GameManager.Instance.LoadSchoolMap();
    }
}
