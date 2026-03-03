using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manager for handling all sound effects in the game. 
/// Provides methods to play specific sounds.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    void Start(){
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else { Instance = this; }
    }

    [Tooltip("Audio clip for the sound of flipping a book")]
    [SerializeField] public AudioClip BookFlip;
    [SerializeField] public List<AudioClip> Footsteps;
    [Tooltip("Reference to the soundObject on the player to play sounds from")]
    [SerializeField] public GameObject SoundObject; 

    public void PlaySound(AudioClip clip, Transform source){
        AudioSource.PlayClipAtPoint(clip, source.position);
    }
    public void PlaySound(AudioClip clip){
        AudioSource.PlayClipAtPoint(clip, SoundObject.transform.position);
    }
    /// <summary>
    /// Used by buttons that need to play a sound.
    /// </summary>
    public void PlayBookFlipSound(){
        PlaySound(BookFlip, SoundObject.transform);
    }
    /// <summary>
    /// Plays a random footstep sound from the list of footstep sounds. Called from the playercontroller.
    /// </summary>
    public void PlayFootstepSound(){
        PlaySound(Footsteps[Random.Range(0, Footsteps.Count)], SoundObject.transform);
    }
}
