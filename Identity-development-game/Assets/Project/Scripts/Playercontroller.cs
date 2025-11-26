using UnityEngine;
using Terresquall;

public class Playercontroller : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public int JID = 1;
    [SerializeField] public GameObject player;
    private Rigidbody body;
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject canvas;
    // All at zDistance depth
    private Vector3 screenBoundsMin; // x,y,z
    private Vector3 screenBoundsMax;
    private float zDistance = 0f; // distance from camera to the object (used with ScreenToWorldPoint)

    void Start(){
        body = GetComponent<Rigidbody>();
        if (cam == null) cam = Camera.main;

        // compute the distance we should project screen corners at.
        // ScreenToWorldPoint expects a Z distance from the camera, so take the absolute
        // distance from the camera to the object's current Z (if the object is set)
        // if (player != null){
        //     zDistance = Mathf.Abs(cam.transform.position.z - player.transform.position.z);
        //     // Safety for perspective cameras: if the object shares the same Z as camera,
        //     // use a small positive distance so ScreenToWorldPoint doesn't return camera position
        //     if (Mathf.Approximately(zDistance, 0f)) zDistance = Mathf.Max(zDistance, cam.nearClipPlane + 0.01f);
        // }

        getScreenBounds();
    }

    void Update(){
        float h = VirtualJoystick.GetAxis("Horizontal", JID);
        float v = VirtualJoystick.GetAxis("Vertical", JID);

        // move in world space using speed and delta time
        Vector3 delta = new Vector3(h * speed, v * speed, 0f);
        if (player != null){
            player.transform.position += delta;
            // Clamp the player inside the precomputed world bounds
            Vector3 p = player.transform.position;
            p.x = Mathf.Clamp(p.x, screenBoundsMin.x, screenBoundsMax.x);
            p.y = Mathf.Clamp(p.y, screenBoundsMin.y, screenBoundsMax.y);
            player.transform.position = p;
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
