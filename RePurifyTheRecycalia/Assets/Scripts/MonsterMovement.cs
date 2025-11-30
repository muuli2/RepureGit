using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 2f;
    public float followRadius = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 moveDir;
    private GameObject player;
    private Vector2 spawnPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                anim.SetFloat("Speed", 0);
                return;
            }
        }

        Vector2 monsterPos = rb.position;
        Vector2 playerPos = player.transform.position;

        float distanceToPlayer = Vector2.Distance(monsterPos, playerPos);

        if (distanceToPlayer <= followRadius)
        {
            moveDir = (playerPos - monsterPos).normalized;
        }
        else
        {
            float distanceToSpawn = Vector2.Distance(monsterPos, spawnPosition);
            if (distanceToSpawn > 0.1f)
                moveDir = (spawnPosition - monsterPos).normalized;
            else
                moveDir = Vector2.zero;
        }

        // ส่งค่าให้อนิเมชัน
        anim.SetFloat("moveX", moveDir.x);
        anim.SetFloat("moveY", moveDir.y);
        anim.SetFloat("Speed", moveDir.magnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
