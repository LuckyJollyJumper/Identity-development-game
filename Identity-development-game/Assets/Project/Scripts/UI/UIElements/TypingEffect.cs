using UnityEngine;
using System.Collections;
using TMPro; // Import TextMeshPro

public class TypingEffect : MonoBehaviour
{
    [HideInInspector] public TMPro.TextMeshProUGUI TextField; // Reference to the TextMeshPro component
    [SerializeField] public float TypingSpeed = 0.1f; // Speed of typing in seconds
    private string FullText; // The complete text to be typed

    public void Start(){
        this.TextField = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void StartEffect(){
        this.FullText = TextField.text; // Store the full text
        this.TextField.text = string.Empty; // Clear the text
        StartCoroutine(TypeText()); // Start typing animation
    }

    // Coroutine to simulate typing effect
    IEnumerator TypeText(){
        foreach (char letter in this.FullText){
            this.TextField.text += letter; // Append each letter to the text
            yield return new WaitForSeconds(this.TypingSpeed); // Wait for the specified duration
        }
        StopCoroutine("TypeText");
    }
}