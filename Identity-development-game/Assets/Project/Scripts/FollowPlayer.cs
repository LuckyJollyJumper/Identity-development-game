using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public float followSpeed = 5f; // adjust for smoothness (higher = faster follow)

    void Update(){    
        if (player != null){
            Vector3 targetPos = new Vector3(player.position.x, 4.0f, player.position.z-3.5f);
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}