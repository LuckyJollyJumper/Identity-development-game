using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
    [SerializeField] GameObject parentObject;

    [Header("Billboard (facing camera)")]
    [Tooltip("Higher values rotate faster; use smoothing to avoid sudden flips")]
    [SerializeField] public float rotationSmoothing = 8f;
    [Tooltip("Maximum absolute pitch angle (degrees) for pitch toward camera")]
    [SerializeField] public float maxXAngle = 30f;

    [Header("Spin (local axis)")]
    [Tooltip("Degrees per second to spin around the local axis (0 disables)")]
    [SerializeField] public float spinSpeed = 0f;
    [Tooltip("Local axis to spin around (in local space)")]
    [SerializeField] public Vector3 spinAxis = Vector3.forward;
    [SerializeField] public float spinAngle = 0f;
    private Transform textBubble; // The target the text bubble will follow
    private Camera mainCamera; // Reference to the main camera

    [Header("References")]
    [SerializeField] TMP_Text bubbleText;

    [Header("Debug")]
    [SerializeField] public bool DebugMode = false;
    private string DebugID;

    void Awake(){
        this.mainCamera = Camera.main;
        this.textBubble = GetComponent<Transform>();
        if (DebugMode){
            DebugID = $"[{parentObject.name}/TextBubble]";
        }
    }

    public void SetBubbleText(string newText){
        if (DebugMode){ Debug.Log($"{DebugID} Setting bubble text to: {newText}, {bubbleText}"); }
        if (bubbleText == null){
            this.bubbleText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
        this.bubbleText.text = newText;
    }
    // LateUpdate is called after all Update methods — for camera-facing logic
    void LateUpdate(){
        Vector3 directionToCamera = mainCamera.transform.position - textBubble.position;
        if (directionToCamera.sqrMagnitude < Mathf.Epsilon) return;

        Quaternion targetRot = Quaternion.LookRotation(directionToCamera);

        // Always clamp pitch (X) toward camera, limited by maxXAngle
        Vector3 e = targetRot.eulerAngles;
        float px = e.x;
        if (px > 180f) px -= 360f;
        px = Mathf.Clamp(px, -Mathf.Abs(maxXAngle), Mathf.Abs(maxXAngle));
        e.x = px;
        targetRot = Quaternion.Euler(e);

        // Apply smoothing only to the billboard (facing) rotation so spin remains consistent
        Quaternion baseRotation;
        if (rotationSmoothing > 0f)
            baseRotation = Quaternion.Slerp(textBubble.rotation, targetRot, Mathf.Clamp01(rotationSmoothing * Time.deltaTime));
        else
            baseRotation = targetRot;

        // Update spin angle and wrap to avoid overflow
        if (Mathf.Abs(spinSpeed) > Mathf.Epsilon){
            spinAngle += spinSpeed * Time.deltaTime;
            if (spinAngle > 360f || spinAngle < -360f) spinAngle = spinAngle % 360f;
        }

        Quaternion spinQuat = Quaternion.identity;
        if (Mathf.Abs(spinSpeed) > Mathf.Epsilon && spinAxis != Vector3.zero){
            spinQuat = Quaternion.AngleAxis(spinAngle, spinAxis.normalized);
        }

        // Compose final rotation: face camera, then spin around local axis
        textBubble.rotation = baseRotation * spinQuat;
    }
}
