using UnityEngine;

/// <summary>
/// Used to be able to work at the world when the world is orientated more easily
/// </summary>
public class SchoolMap : MonoBehaviour
{
    void Start(){
        this.transform.Rotate(0f,45f,0f);
    }
}
