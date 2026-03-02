using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for handling scene transitions in the game. Provides methods to load specific scenes, reload the current scene, and quit the game. 
/// Scenes are defined in an enum in the order as they are set in the File>Build profiles in the unity editor.
/// Is accessed from the GameManager to also enable saving
/// </summary>
public class ScenesManager : MonoBehaviour{
    [HideInInspector] public static ScenesManager Instance;

    public enum Scenes{
        SchoolMap, // Main map to navigate the school and access different areas
        CharacterCreator, // Sets name and character
        PersonaGame, // Minigame to find out the persona
        PuzzleMiniGame, // Minigame to solve puzzles
        ClothingMiniGame, // Minigame to sort clothing pieces
        SportMiniGame,
    }

    public void Awake(){ Instance = this; }
    public void LoadScene(Scenes scene){
        SceneManager.LoadScene(scene.ToString());
    }
    public void QuitGame(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;// TODO: Remove this line for build!!!!!!!!!!!
    }
}
