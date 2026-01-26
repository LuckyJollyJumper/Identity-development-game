using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform Player;
    [SerializeField] public float FollowSpeed = 5f; // adjust for smoothness (higher = faster follow)

    void Update(){    
        if (Player != null){
            Vector3 targetPos = new Vector3(Player.position.x, 9.0f, Player.position.z-3.5f);
            transform.position = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
        }
    }
}