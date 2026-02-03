using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used on the progressbar prefab that represents a percentage value visually
/// </summary>
public class Progressbar : MonoBehaviour
{
    private float ProgressSize;
    private RectTransform ProgressBar; // The bar that will increase or shrink in size
    private RectTransform ProgressBack; // The size of the full progressbar
    void Start(){
        this.ProgressBack = GetComponent<RectTransform>();
        this.ProgressBar = transform.Find("Progressbar").GetComponent<RectTransform>();
        // Get the width of the full progressbar (100%)
        ProgressSize = ProgressBack.sizeDelta.x;
    }

    /// <summary>
    /// Sets the size of Progressbar given the percentage. It assumes percentage = 1f is 100%.
    /// </summary>
    /// <param name="percentage"></param>
    public void SetProgress(float percentage){
        ProgressBar.sizeDelta.x = ProgressSize * percentage;
    }

}
