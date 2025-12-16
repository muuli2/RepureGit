using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossMap01 : MonoBehaviour
{
    public static BossMap01 Instance;

    [Header("Stats")]
    public int maxHealth = 20;
    private int currentHealth;

    public enum BossState { Normal, WaitingMinigame, Dead }
    public BossState state = BossState.Normal;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseDistance = 6f;
    public float attackDistance = 1.5f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    private float attackTimer = 0f;
    public int damage = 1;
    public GameObject waterPrefab;
    public float waterRange = 2f;
    public float waterDuration = 0.6f;
     public float attackRange = 1f;
      public Transform attackPoint;

    [Header("Animator / FX")]
    public Animator anim;
    public SpriteRenderer sr;
    public Image healthBarFill;

    [Header("Player & Map")]
    public Transform player;
    private Rigidbody2D rb;
      public LayerMask playerLayer;

    [Header("Minigame")]
    public string miniGameSceneName = "Minigame01";

    [Header("SFX")]
public AudioSource audioSource;
public AudioClip showdownClip;


    private bool isDead = false;
     private Collider2D col;
       private Vector3 startPosition;

    private void Awake()
    {

          Instance = this; // เพิ่มบรรทัดนี้
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        state = BossState.Normal;
        isDead = false;
        startPosition = transform.position;
        currentHealth = maxHealth;
        UpdateHealthBar();


    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
         if (player == null)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }
        if (isDead) return;

        if (state == BossState.WaitingMinigame) return;

        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        // sr.flipX = player.position.x < transform.position.x;

        if (dist <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else if (dist <= chaseDistance)
        {
            MoveToPlayer();
        }
        else
        {
            Idle();
        }
    }

   void Idle()
{
    rb.linearVelocity = Vector2.zero;
    anim.SetFloat("moveX", 0);
    anim.SetFloat("moveY", 0);
    anim.SetFloat("Speed", 0);
}

  void MoveToPlayer()
{
    Vector2 dir = (player.position - transform.position).normalized;
    rb.linearVelocity = dir * moveSpeed;

    anim.SetFloat("moveX", dir.x);
    anim.SetFloat("moveY", dir.y);
    anim.SetFloat("Speed", dir.magnitude);

    Debug.Log($"dir.x = {dir.x}, flipX = {sr.flipX}");
}


private void FreezeBoss()
{
    rb.linearVelocity = Vector2.zero;
    rb.angularVelocity = 0f;
    rb.bodyType = RigidbodyType2D.Kinematic;
}


  void Attack()
{
    rb.linearVelocity = Vector2.zero;     // หยุดเดิน
    anim.SetTrigger("attack");      // เล่นแอนิเมชันโจมตี
}

    public void AttackWater()
    {
        SpawnWater(Vector2.up);
        SpawnWater(Vector2.down);
        SpawnWater(Vector2.left);
        SpawnWater(Vector2.right);
    }

    void SpawnWater(Vector2 dir)
    {
        Vector3 pos = transform.position + (Vector3)(dir * waterRange);
        GameObject w = Instantiate(waterPrefab, pos, Quaternion.identity);
        Destroy(w, waterDuration);
    }

    // ---------------- Damage System ----------------
    public void TakeDamage(int dmg)
    {
        if (state != BossState.Normal || isDead) return;

        currentHealth -= dmg;
        anim.SetTrigger("hurt");
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            StartCoroutine(TriggerShowdown());
        }
    }

    public void DealDamage()
{
    Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
    if(hit != null && hit.CompareTag("Player"))
    {
        GameManager.Instance.TakeDamage(damage);
    }
}

    void UpdateHealthBar()
    {
        if (healthBarFill)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    // ---------------- Showdown → Minigame ----------------
   private IEnumerator TriggerShowdown()
    {
        state = BossState.WaitingMinigame;
        FreezeBoss(); 

        // FreezeMapAndPlayer();

        // glowEffect?.SetActive(true);
        // bossAnimator?.SetTrigger("PhaseTransition");

       IntroMinigame tt = FindFirstObjectByType<IntroMinigame>();
if (tt != null)
{
    if (audioSource && showdownClip)
        audioSource.PlayOneShot(showdownClip);

    yield return tt.ShowText("SHOWDOWN");
}

        // GameManager.Instance.isMiniGameActive = true;

        if (GameManager.Instance != null)
            GameManager.Instance.isMiniGameActive = true;

            

            

        // bgmSource?.Stop();

        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }

    public void MinigameWin()
    {
        // กลับมาบอสแมพ → Trigger Die
         BossDefeated();
    }

   public void BossDefeated()
    {
        if (state == BossState.Dead) return;

        state = BossState.Dead;
        isDead = true;

        ScoreManage.Instance?.AddScore(1000);

        // bgmSource?.Play();

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;

        // glowEffect?.SetActive(false);
        anim?.SetTrigger("die");

        StartCoroutine(FinishBossDeath(2f));
    }

    private IEnumerator FinishBossDeath(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // UnfreezeMapAndPlayer();

        if (MonsterManage.Instance != null)
            MonsterManage.Instance.EnemyKilled();

        Destroy(gameObject);
    }

public void ResetBoss()
{
    isDead = false;
    state = BossState.Normal;
    currentHealth = maxHealth;
    UpdateHealthBar();

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if(rb != null)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
    }

    foreach (var c in GetComponentsInChildren<Collider2D>())
        c.enabled = true;

    if(anim != null)
        anim.Rebind();
}



// Coroutine จบขั้นตอนบอสตาย
// private IEnumerator FinishBossDeath(float delay)
// {
//     yield return new WaitForSeconds(delay);

//     // ถ้ามี map freeze/unfreeze สามารถเรียกตรงนี้ได้
//     // UnfreezeMapAndPlayer();

//     // แจ้งระบบมอนสเตอร์ว่าตายแล้ว
//     if (MonsterManage.Instance != null)
//         MonsterManage.Instance.EnemyKilled();

//     // ทำลายบอส
//     Destroy(gameObject);
// }
    // void FreezeMapAndPlayer()
    // {
    //     foreach (var rb2 in FindObjectsOfType<Rigidbody2D>())
    //     {
    //         if (rb2.CompareTag("Player")) continue;
    //         rb2.velocity = Vector2.zero;
    //         rb2.bodyType = RigidbodyType2D.Static;
    //     }

    //     var pm = player.GetComponent<PlayerMovement>();
    //     if (pm) pm.enabled = false;
    //     var prb = player.GetComponent<Rigidbody2D>();
    //     if (prb) prb) prb.velocity = Vector2.zero;
    // }

    // void UnfreezeMapAndPlayer()
    // {
    //     foreach (var rb2 in FindObjectsOfType<Rigidbody2D>())
    //     {
    //         if (rb2.CompareTag("Player")) continue;
    //         rb2.bodyType = RigidbodyType2D.Dynamic;
    //     }

    //     var pm = player.GetComponent<PlayerMovement>();
    //     if (pm) pm.enabled = true;
    // }

   private void OnDrawGizmosSelected()
{
    // 🟡 ระยะไล่ (Chase)
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, chaseDistance);

    // 🔵 ระยะเริ่มโจมตี (Attack Distance)
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, attackDistance);

    // 🔴 ระยะโดนจริง (Attack Range)
    if (attackPoint != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // 🟢 จุดโจมตี (Attack Point)
    if (attackPoint != null)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(attackPoint.position, 0.05f);
    }
}


    
}

