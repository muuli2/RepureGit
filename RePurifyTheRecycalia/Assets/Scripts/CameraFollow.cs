using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                 // ตัวละครที่กล้องตาม
    public Vector3 offset = new Vector3(0, 3, -7); // ปรับตำแหน่งกล้อง
    public float smoothSpeed = 0.25f;

    private Vector3 velocity = Vector3.zero;

    public float zoom = 7f;                  // ค่า zoom (สำหรับกล้อง 2D orthographic)

    void LateUpdate()
    {
        if (target == null) return;

         Vector3 desiredPosition = target.position + offset;
    transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
       
        // ซูมกล้อง 2D
        Camera.main.orthographicSize = zoom;
    }
}
