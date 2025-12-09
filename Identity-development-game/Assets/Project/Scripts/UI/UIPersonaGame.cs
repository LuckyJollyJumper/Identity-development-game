using UnityEngine;

public class UIPersonaGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSubmitPersona(){
        GameManager.Instance.LoadScene(ScenesManager.scenes.MainMenu);
    }
}
