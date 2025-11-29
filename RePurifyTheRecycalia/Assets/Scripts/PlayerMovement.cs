using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 movement;

    private bool canMove = true;  // 👈 เพิ่ม flag

    // ฟังก์ชันเช็คว่ามีการ input
    public bool IsMoving()
    {
        return movement.sqrMagnitude > 0.01f; 
    }

    // --- ฟังก์ชันควบคุมการ freeze/unfreeze ---
    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove) rb.linearVelocity = Vector2.zero; // หยุดทันที
    }

    void Update()
    {
        if (!canMove) 
        {
            movement = Vector2.zero;
            return;
        }

        movement.x = (Keyboard.current.aKey.isPressed ? -1 : 0)
                   + (Keyboard.current.dKey.isPressed ? 1 : 0);

        movement.y = (Keyboard.current.sKey.isPressed ? -1 : 0)
                   + (Keyboard.current.wKey.isPressed ? 1 : 0);
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
