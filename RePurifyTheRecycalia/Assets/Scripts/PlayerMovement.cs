using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 movement;

    private bool canMove = true;
    private Animator anim;

    // ใช้สำหรับ Idle ให้หันถูกทิศเมื่อปล่อยปุ่ม
    private Vector2 lastMoveDir;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public bool IsMoving()
    {
        return movement.sqrMagnitude > 0.01f;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            anim.SetBool("isWalking", false);
            return;
        }

        // WASD กดพร้อมกันได้
        movement.x = (Keyboard.current.aKey.isPressed ? -1 : 0)
                   + (Keyboard.current.dKey.isPressed ? 1 : 0);

        movement.y = (Keyboard.current.sKey.isPressed ? -1 : 0)
                   + (Keyboard.current.wKey.isPressed ? 1 : 0);

        // จำกัดให้ไม่เกิน 1 เพื่อไม่ให้เร็วตอนกดทแยง
        movement = movement.normalized;

        anim.SetBool("isWalking", IsMoving());

        // ส่งค่าให้ BlendTree
        anim.SetFloat("inputX", movement.x);
        anim.SetFloat("inputY", movement.y);

        // เก็บทิศสุดท้ายที่เดินไว้ใช้กับ Idle
        if (IsMoving())
        {
            lastMoveDir = movement;
            anim.SetFloat("LastInputX", lastMoveDir.x);
            anim.SetFloat("LastInputY", lastMoveDir.y);
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
