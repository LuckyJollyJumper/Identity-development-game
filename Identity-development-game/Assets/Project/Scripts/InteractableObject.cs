using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public GameObject interactionObject;
    public virtual void OnInteract()
    {
        this.interactionObject.SetActive(true);
    }
}
