using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 2f;            // ความเร็ว
    public float followRadius = 5f;     // รัศมีตาม Player
    private Rigidbody2D rb;
    private Vector2 moveDir;

    private GameObject player;
    private Vector2 spawnPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position; // เก็บจุด Spawn
    }

    void Update()
    {
        // หา Player ทุก Update (กรณี Spawn ทีหลัง)
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) return; // ถ้าไม่มี Player
        }

        Vector2 monsterPos = rb.position;                  // แปลง Monster เป็น Vector2
        Vector2 playerPos = player.transform.position;    // แปลง Player เป็น Vector2

        float distanceToPlayer = Vector2.Distance(monsterPos, playerPos);

        if (distanceToPlayer <= followRadius)
        {
            // เดินตาม Player
            moveDir = (playerPos - monsterPos).normalized;
        }
        else
        {
            // เดินกลับ Spawn
            float distanceToSpawn = Vector2.Distance(monsterPos, spawnPosition);
            if (distanceToSpawn > 0.1f)
            {
                moveDir = (spawnPosition - monsterPos).normalized;
            }
            else
            {
                moveDir = Vector2.zero; // ถึง Spawn แล้วหยุด
            }
        }
    }

    void FixedUpdate()
    {
        if (moveDir != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
