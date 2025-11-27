using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 movement;

    // ฟังก์ชันเช็คว่ามีการ input
    public bool IsMoving()
    {
        return movement.sqrMagnitude > 0.01f; // ถ้ามีการ input เล็กน้อยก็ถือว่ากำลังขยับ
    }

    void Update()
    {
        movement.x = (Keyboard.current.aKey.isPressed ? -1 : 0)
                   + (Keyboard.current.dKey.isPressed ? 1 : 0);

        movement.y = (Keyboard.current.sKey.isPressed ? -1 : 0)
                   + (Keyboard.current.wKey.isPressed ? 1 : 0);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
