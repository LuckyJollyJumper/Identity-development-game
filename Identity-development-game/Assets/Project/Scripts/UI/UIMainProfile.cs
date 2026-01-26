using UnityEngine;

/// <summary>
/// UI class that controls all the UI elements for the profile of the player. Is included into the locker
/// interactable object.
/// </summary>
public class UIMainProfile : MonoBehaviour
{
    
    [SerializeField] public GameObject Locker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// Function used by the Quitbutton in the UI
    /// </summary>
    public void CloseLocker(){
        Locker.GetComponent<InteractableObject>().OnEndInteract();
    }
}
