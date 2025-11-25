using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 move;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
            Debug.LogError("Rigidbody2D ไม่พบใน PlayerController!");
    }

    // **ชื่อฟังก์ชันตรงกับชื่อ Action ใน Input System**
    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        // Debug.Log("Move: " + move); // สามารถเปิดดูค่า input
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

        if (move.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(move.x), 1, 1);
    }
}
