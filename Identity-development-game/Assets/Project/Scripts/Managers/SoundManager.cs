using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    [SerializeField] public AudioClip BookFlip; // Array of all the sound clips used in the game
    [SerializeField] public List<AudioClip> Footsteps;
    [SerializeField] public GameObject SoundObject; // Reference to the soundObject on the player to play sounds from

    public void PlaySound(AudioClip clip, Transform source){
        AudioSource.PlayClipAtPoint(clip, source.position);
    }
    public void PlaySound(AudioClip clip){
        AudioSource.PlayClipAtPoint(clip, SoundObject.transform.position);
    }

    public void PlayBookFlipSound(){
        PlaySound(BookFlip, SoundObject.transform);
    }
    public void PlayFootstepSound(){
        PlaySound(Footsteps[Random.Range(0, Footsteps.Count)], SoundObject.transform);
    }
}
