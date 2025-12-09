using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalBoss : MonoBehaviour
{
    public static FinalBoss Instance;

    [Header("Ranged Attack")]
public GameObject bulletPrefab;
public Transform bulletSpawnPoint;
public float bulletSpeed = 5f;
public float rangeAttackDistance = 4f;   // ระยะเริ่มยิงกระสุน




    // -------------------------------------
    // 🔥 Stats
    // -------------------------------------
    [Header("Stats")]
    public int maxHealth = 20;
    private int currentHealth;

    public enum BossState { Normal, WaitingMinigame, Dead }
    public BossState state = BossState.Normal;

    // -------------------------------------
    // 🧭 Movement & Attack
    // -------------------------------------
    [Header("Movement / Attack")]
    public float moveSpeed = 2f;
    public float chaseDistance = 6f;
    public float attackDistance = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    private float attackTimer = 0f;

    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask playerLayer;

    private Transform player;
    private Rigidbody2D rb;
    private bool isDead = false;
    private SpriteRenderer sr;
    public Transform firePoint;

    [Header("Special Attack")]
public float specialAttackCooldown = 5f;   // ยิงกระจายทุก 5 วิ
private float specialAttackTimer = 0f;
public float spreadDetectDistance = 5f; 

public int spreadBulletCount = 12; // จำนวนกระสุนรอบตัว (12 ทิศ)
public float spreadBulletSpeed = 4f;


    // -------------------------------------
    // UI / FX
    // -------------------------------------
    [Header("UI / FX")]
    public Image healthBarFill;
    public Animator bossAnimator;
    public GameObject glowEffect;
    public AudioSource bgmSource;

    [Header("Minigame")]
    public string miniGameSceneName = "MiniGameFinal";


    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
          sr = GetComponent<SpriteRenderer>();  

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    private void Update()
{
    if (state != BossState.Normal || isDead) 
    {
        Idle();
        return;
    }

    if (player == null)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
        {
            Idle();
            return;
        }
    }

    // ลดค่า cooldown
    if (attackTimer > 0)
        attackTimer -= Time.deltaTime;
        // นับเวลาสำหรับยิงรอบตัว
if (specialAttackTimer > 0)
    specialAttackTimer -= Time.deltaTime;


    float dist = Vector2.Distance(transform.position, player.position);

    // ⭐ ยิงกระสุนกระจายรอบตัวทุก 5 วิ
// ⭐ ยิงกระจายเฉพาะเมื่อผู้เล่นอยู่ในระยะที่กำหนด
if (dist <= spreadDetectDistance)
{
    if (specialAttackTimer <= 0f)
    {
        ShootSpread();
        specialAttackTimer = specialAttackCooldown;
    }
}
else
{
    // ถ้าออกจากระยะ → reset timer เพื่อไม่ให้กลับมาแล้วยิงทันที
    specialAttackTimer = specialAttackCooldown;
}



    // >>>>>>>>>>>> เพิ่มฟลิบหันซ้าย–ขวา <<<<<<<<<<<<<<
    if (player != null)
    {
        if (player.position.x > transform.position.x)
            sr.flipX = false;  // หันขวา
        else
            sr.flipX = true;   // หันซ้าย
    }
    // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    // ระยะประชิด
    if (dist <= attackDistance)
    {
        rb.linearVelocity = Vector2.zero;
        bossAnimator.SetBool("isWalking", false);

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }
    // ระยะยิงไกล
    else if (dist <= rangeAttackDistance)
    {
        rb.linearVelocity = Vector2.zero;
        bossAnimator.SetBool("isWalking", false);

        if (attackTimer <= 0f)
        {
            bossAnimator.SetTrigger("shoot");
            attackTimer = attackCooldown;
        }
    }
    // ไล่ผู้เล่น
    else if (dist <= chaseDistance)
    {
        ChasePlayer();
    }
    // นิ่ง
    else
    {
        Idle();
    }
}

void ShootSpread()
{
    float angleStep = 360f / spreadBulletCount;
    float angle = 0f;

    for (int i = 0; i < spreadBulletCount; i++)
    {
        float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
        float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

        Vector2 dir = new Vector2(dirX, dirY).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();

        brb.linearVelocity = dir * spreadBulletSpeed;

        angle += angleStep;
    }
}


    // -------------------------------------
    // 😐 Idle
    // -------------------------------------
    void Idle()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (bossAnimator != null)
            bossAnimator.SetBool("isWalking", false);
    }

    // -------------------------------------
    // 🏃 Chase
    // -------------------------------------
    void ChasePlayer()
    {
        if (player == null) return;

        bossAnimator.SetBool("isWalking", true);

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    // -------------------------------------
    // ⚔ Attack (Animation Trigger)
    // -------------------------------------
    void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        bossAnimator.SetTrigger("attack");

        
    }

    // เรียกจาก Animation Event
    public void DealDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            GameManager.Instance?.TakeDamage(damage);
        }
    }

    public void ShootBullet()
{
    if (player == null) return;

    GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    FinalBossBullet bullet = b.GetComponent<FinalBossBullet>();

    // ทิศไปหา player จริง
    Vector2 dir = (player.position - firePoint.position).normalized;

    bullet.SetDirection(dir);

    // ฟลิปภาพกระสุน ถ้ามี sprite
    SpriteRenderer sr = b.GetComponent<SpriteRenderer>();
    if (sr != null)
        sr.flipX = (dir.x < 0);
}



    // -------------------------------------
    // ❤️ Take Damage
    // -------------------------------------
    public void TakeDamage(int dmg)
    {
        if (state != BossState.Normal || isDead) return;

        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (bossAnimator != null)
            bossAnimator.SetTrigger("hurt");

        UpdateHealthBar();

        if (currentHealth <= 0)
            StartCoroutine(TriggerMinigameTransition());
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    // -------------------------------------
    // 🔥 Freeze before Minigame
    // -------------------------------------
    private IEnumerator TriggerMinigameTransition()
    {
        state = BossState.WaitingMinigame;

        FreezeMapAndPlayer();

        // glowEffect?.SetActive(true);
        // bossAnimator?.SetTrigger("PhaseTransition");

        IntroMinigame tt = FindFirstObjectByType<IntroMinigame>();
        if (tt != null)
            yield return tt.ShowText("FINALSHOWDOWN");

        yield return new WaitForSecondsRealtime(0.5f);

        GameManager.Instance.isMiniGameActive = true;

        bgmSource?.Stop();

        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }


    // -------------------------------------
    // ❄ Freeze/Unfreeze
    // -------------------------------------
    void FreezeMapAndPlayer()
    {
        foreach (var rb in FindObjectsOfType<Rigidbody2D>())
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerMovement pm = p.GetComponent<PlayerMovement>();
            if (pm) pm.enabled = false;

            Rigidbody2D prb = p.GetComponent<Rigidbody2D>();
            if (prb)
            {
                prb.linearVelocity = Vector2.zero;
                prb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    void UnfreezeMapAndPlayer()
    {
        foreach (var rb in FindObjectsOfType<Rigidbody2D>())
            rb.bodyType = RigidbodyType2D.Dynamic;

        foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerMovement pm = p.GetComponent<PlayerMovement>();
            if (pm) pm.enabled = true;
        }
    }

    // -------------------------------------
    // 🪦 Boss Defeated
    // -------------------------------------
    public void BossDefeated()
    {
        if (state == BossState.Dead) return;

        state = BossState.Dead;
        isDead = true;

        ScoreManage.Instance?.AddScore(1000);

        bgmSource?.Play();

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;

        glowEffect?.SetActive(false);
        bossAnimator?.SetTrigger("Die");

        StartCoroutine(FinishBossDeath(2f));
    }

    private IEnumerator FinishBossDeath(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        UnfreezeMapAndPlayer();
        MonsterManage.Instance?.EnemyKilled();

        Destroy(gameObject);
    }

    // -------------------------------------
    // 🧪 Debug Gizmos
    // -------------------------------------
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
