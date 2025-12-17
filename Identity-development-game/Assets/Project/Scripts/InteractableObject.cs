using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public virtual void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
