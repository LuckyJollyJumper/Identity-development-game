using UnityEngine;

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

    public void PlaySound(AudioClip clip, Transform source){
        AudioSource.PlayClipAtPoint(clip, source.position);
    }
}
