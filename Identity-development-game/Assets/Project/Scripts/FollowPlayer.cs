using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]public Transform player;

    // Update is called once per frame
    void Update () {
        if (player != null) transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}
