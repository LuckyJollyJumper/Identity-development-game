using UnityEngine;

public class UIMainProfile : MonoBehaviour
{
    
    [SerializeField] public GameObject locker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void CloseLocker(){
        gameObject.SetActive(false);
    }
}
