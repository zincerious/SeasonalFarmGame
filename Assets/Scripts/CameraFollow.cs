using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
    }
}
