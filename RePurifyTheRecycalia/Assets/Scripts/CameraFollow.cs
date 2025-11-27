using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                 // ตัวละครที่กล้องตาม
    public Vector3 offset = new Vector3(0, 3, -7); // ปรับตำแหน่งกล้อง
    public float smoothSpeed = 5f;

    public float zoom = 7f;                  // ค่า zoom (สำหรับกล้อง 2D orthographic)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // ซูมกล้อง 2D
        Camera.main.orthographicSize = zoom;
    }
}
