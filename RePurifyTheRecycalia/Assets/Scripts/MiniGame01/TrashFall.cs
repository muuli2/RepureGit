using UnityEngine;

public class TrashFall : MonoBehaviour
{
    public float fallSpeed = 1f;  // ปรับช้าเร็วได้

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
}
