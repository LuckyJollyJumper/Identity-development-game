using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used on the progressbar prefab that represents a percentage value visually
/// </summary>
public class Progressbar : MonoBehaviour
{
    private float ProgressSize;
    [Header("References")]
    [SerializeField] private RectTransform ProgressBar; // The bar that will increase or shrink in size
    private RectTransform ProgressBack; // The size of the full progressbar
    void Start(){
        this.ProgressBack = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Sets the size of Progressbar given the percentage. It assumes percentage = 1f is 100%.
    /// </summary>
    /// <param name="percentage"></param>
    public void SetProgress(float percentage){
        // Calculate the width based on percentage (0 at 0%, full width at 100%)
        ProgressBar.localScale = new Vector3(percentage, ProgressBar.localScale.y, ProgressBar.localScale.z);
    }

}
