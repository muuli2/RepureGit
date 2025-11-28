using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        // หา Animator แล้ว Destroy หลังจบอนิเมชัน
        Animator anim = GetComponent<Animator>();
        if(anim != null)
        {
            float animLength = anim.runtimeAnimatorController.animationClips[0].length;
            Destroy(gameObject, animLength);
        }
        else
        {
            Destroy(gameObject, 3f); // fallback
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Monster monster = col.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
