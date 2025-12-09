using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float maxDistance = 10f; // ← ระยะยิงสูงสุด

    private Rigidbody2D rb;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        startPos = transform.position;

        // ทำลายตัวเองหลังจบ Animation (optional)
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            float animLength = anim.runtimeAnimatorController.animationClips[0].length;
            Destroy(gameObject, animLength);
        }
        else
        {
            Destroy(gameObject, 3f); // fallback
        }
    }

    void Update()
    {
        // ตรวจสอบระยะ
        float traveled = Vector3.Distance(startPos, transform.position);
        if (traveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // ตรวจ Monster
        Monster monster = col.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // ตรวจ Boss
        Boss boss = col.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }

         FinalBoss finalboss = col.GetComponent<FinalBoss>();
        if (finalboss != null)
        {
            finalboss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
