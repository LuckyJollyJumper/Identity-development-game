using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public GameObject interactionObject;
    [SerializeField] public GameObject UI;
    public virtual void OnInteract()
    {
        this.interactionObject.SetActive(true);
        this.UI.SetActive(true);
    }
}
