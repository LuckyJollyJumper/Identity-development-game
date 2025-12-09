using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour{
    [HideInInspector] public static ScenesManager Instance;

    public enum scenes{
        MainMenu, // Should check if a player is active and then use the player data and stay or go to the character creator
        CharacterCreator, // Sets name and character
        PersonaGame, // Minigame to find out the persona
        StoryIntro, // Intro to the story and interaction with first character
    }

    public void Awake(){
        Instance = this;
    }

    public void LoadScene(scenes scene){
        SceneManager.LoadScene(scene.ToString());
    }
    public void ReloadCurrentScene(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void QuitGame(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;// TODO: Remove this line for build!!!!!!!!!!!
    }
}
