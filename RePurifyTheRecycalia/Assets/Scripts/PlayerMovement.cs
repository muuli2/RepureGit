using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private bool isDashing = false;
    private float dashTime;
    public float dashCDTimer;

    [HideInInspector]
    public Vector2 movement;

    private bool canMove = true;
    private Animator anim;

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
        // ลดคูลดาวน์ Dash
        if (dashCDTimer > 0)
            dashCDTimer -= Time.deltaTime;

        // ---------------------
        // ปุ่ม Dash = Shift
        // ---------------------
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            !isDashing &&
            dashCDTimer <= 0 &&
            movement != Vector2.zero) // ต้องมีทิศเดิน ถึงแดชได้
        {
            StartDash();
        }

        if (isDashing) return;

        // ------------------------------------
        // การเคลื่อนที่ปกติ (เดิน)
        // ------------------------------------
        if (!canMove)
        {
            movement = Vector2.zero;
            anim.SetBool("isWalking", false);
            return;
        }

        movement.x = (Keyboard.current.aKey.isPressed ? -1 : 0)
                   + (Keyboard.current.dKey.isPressed ? 1 : 0);

        movement.y = (Keyboard.current.sKey.isPressed ? -1 : 0)
                   + (Keyboard.current.wKey.isPressed ? 1 : 0);

        movement = movement.normalized;

        anim.SetBool("isWalking", IsMoving());

        anim.SetFloat("inputX", movement.x);
        anim.SetFloat("inputY", movement.y);

        if (IsMoving())
        {
            lastMoveDir = movement;
            anim.SetFloat("LastInputX", lastMoveDir.x);
            anim.SetFloat("LastInputY", lastMoveDir.y);
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = lastMoveDir * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }

            return;
        }

        if (!canMove) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCDTimer = dashCooldown;

        // ทิศที่พุ่งจะใช้ทิศสุดท้ายที่เดิน
        if (movement != Vector2.zero)
            lastMoveDir = movement;

        // ถ้ามีอนิเมชัน Dash ก็ส่งได้ เช่น
        // anim.SetTrigger("dash");
    }
}
