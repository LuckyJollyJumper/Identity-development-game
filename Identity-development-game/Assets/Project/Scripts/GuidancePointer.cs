using UnityEngine;

/// <summary>
/// Positions this object on a ring around the `player` and points it toward the `target`.
/// Attach this to the pointer object (e.g. arrow, indicator). Set `player` and `target` in the inspector.
/// </summary>
public class GuidancePointer : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform target;

    [Header("Positioning")]
    [SerializeField] public float radius = 2f;
    [SerializeField] public float height = 1f;

    [Header("Smoothing")]
    [SerializeField] public float smoothSpeed = 6f;
    [SerializeField] public bool smoothMovement = true;

    [Header("Rotation")]
    [SerializeField] public bool faceTarget = true;

    [Header("Visibility")]
    [Tooltip("When true, pointer will hide if target is extremely close to the player")]
    [SerializeField] public bool hideWhenClose = false;
    [SerializeField] public float hideDistance = 0.5f;

    void LateUpdate(){
        if (player == null || target == null) return;

        Vector3 toTarget = target.position - player.position;
        Vector3 toTargetFlat = new Vector3(toTarget.x, 0f, toTarget.z);

        if (toTargetFlat.sqrMagnitude < 0.0001f){
            toTargetFlat = player.forward;
            toTargetFlat.y = 0f;
        }

        Vector3 dir = toTargetFlat.normalized;
        Vector3 desiredPos = player.position + dir * radius + Vector3.up * height;

        if (smoothMovement)
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
        else
            transform.position = desiredPos;

        if (faceTarget){
            Vector3 lookDir = (target.position - transform.position);
            if (lookDir.sqrMagnitude > 0.0001f){
                Quaternion desiredRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * smoothSpeed);
            }
        }

        if (hideWhenClose){
            float dist = Vector3.Distance(player.position, target.position);
            bool hide = dist <= hideDistance;
            SetVisible(!hide);
        }
    }

    public void SetVisible(bool visible){
        if (gameObject.activeSelf != visible)
            gameObject.SetActive(visible);
    }

    /// <summary>
    /// Instantly snap pointer to correct position/rotation.
    /// </summary>
    public void Snap(){
        if (player == null || target == null) return;
        Vector3 toTarget = target.position - player.position;
        Vector3 toTargetFlat = new Vector3(toTarget.x, 0f, toTarget.z);
        if (toTargetFlat.sqrMagnitude < 0.0001f) toTargetFlat = player.forward;
        Vector3 dir = toTargetFlat.normalized;
        transform.position = player.position + dir * radius + Vector3.up * height;
        if (faceTarget){
            Vector3 lookDir = (target.position - transform.position);
            if (lookDir.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        }
        
    }
}
