using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour{
    [HideInInspector] public static ScenesManager Instance;

    public enum Scenes{
        SchoolMap, // Main map to navigate the school and access different areas
        CharacterCreator, // Sets name and character
        PersonaGame, // Minigame to find out the persona
        PuzzleMiniGame, // Minigame to solve puzzles
        ClothingMiniGame, // Minigame to sort clothing pieces
    }

    public void Awake(){
        Instance = this;
    }

    public void LoadScene(Scenes scene){
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
