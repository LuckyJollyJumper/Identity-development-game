using UnityEngine;
using Terresquall;

/// <summary>
/// Player controller that moves a player GameObject using a virtual joystick,
/// </summary>
public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed;// 120-150 for normal movement
    [SerializeField] public int JID = 1;
    [SerializeField] public GameObject player;
    private Rigidbody body;
    [SerializeField] public GameObject canvas;

    private Vector3 screenBoundsMin;
    private Vector3 screenBoundsMax;

    void Start(){
        body = player.GetComponent<Rigidbody>();
        getScreenBounds();
    }

    void Update(){
        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);

        // move in world space using speed and delta time
        Vector3 delta = new Vector3(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0f);
        if (player != null){
            // Clamp the player inside the precomputed world bounds
            Vector3 deltaP = player.transform.position + delta;
            deltaP.x = Mathf.Clamp(deltaP.x, screenBoundsMin.x, screenBoundsMax.x);
            deltaP.y = Mathf.Clamp(deltaP.y, screenBoundsMin.y, screenBoundsMax.y);
            player.transform.position = deltaP;
        } 
    }

    /*  
     * Get the world-space screen bounds at the object's z distance from the canvas background and
     * save them to screenBoundsMin and screenBoundsMax
     */
    void getScreenBounds(){
        Vector3[] worldCorners = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
        Vector3 bottomLeft = worldCorners[0]; 
        Vector3 topRight = worldCorners[2]; 
        screenBoundsMin = bottomLeft;
        screenBoundsMax = topRight;

        // screenBoundsMin  = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        // screenBoundsMax  = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));
        print("Screen LB: " + screenBoundsMin + " RU: " + screenBoundsMax);
    } 
} 
