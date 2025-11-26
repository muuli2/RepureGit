using UnityEngine;
using UnityEngine.InputSystem; // ต้องมีอันนี้

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movement;

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
