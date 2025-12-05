using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [Header("Stats")]  
    public int maxHealth = 20;  
    private int currentHealth;  

    public enum BossState { Normal, WaitingMinigame, Dead }  
    public BossState state = BossState.Normal;  

    [Header("UI")]  
    public Image healthBarFill;

    [Header("Effects")]  
    public Animator bossAnimator;
    public GameObject glowEffect;

    [Header("Minigame")]  
    public string miniGameSceneName = "MiniGame01";

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // -------------------------------------
    // ❄ Freeze / Unfreeze
    // -------------------------------------

    void FreezeAllMapObjects()
    {
        Rigidbody2D[] bodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        foreach (var rb in bodies)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Freeze PlayerMovement script
        PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
            player.enabled = false;
    }

    void UnfreezeAllMapObjects()
    {
        Rigidbody2D[] bodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        foreach (var rb in bodies)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Unfreeze PlayerMovement
        PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
            player.enabled = true;
    }

    // -------------------------------------
    // ❤️ Damage / Health
    // -------------------------------------

    public void TakeDamage(int damage)
    {
        if (state != BossState.Normal) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            StartCoroutine(TriggerMinigameTransition());
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    private IEnumerator TriggerMinigameTransition()
{
    state = BossState.WaitingMinigame;

    FreezeAllMapObjects();

    if (glowEffect != null) glowEffect.SetActive(true);
    if (bossAnimator != null) bossAnimator.SetTrigger("PhaseTransition");

    // แสดง SHOWDOWN
    IntroMinigame tt = Object.FindFirstObjectByType<IntroMinigame>();
    if (tt != null)
        yield return tt.ShowText("SHOWDOWN"); // ❌ ต้อง yield รอให้แสดงจบ

    yield return new WaitForSeconds(0.5f); // รอเล็กน้อยก่อนโหลดมินิเกม

    // บอก GameManager ว่าเป็นมินิเกม
    if (GameManager.Instance != null)
        GameManager.Instance.isMiniGameActive = true;

    // โหลดมินิเกม Additive
    SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
}

    // -------------------------------------
    // 💀 Boss Died After Minigame
    // -------------------------------------

    // -------------------------------------
// 💀 Boss Died After Minigame
// -------------------------------------
 public void BossDefeated()
{
    if (state == BossState.Dead) return;
    state = BossState.Dead;

    // ให้คะแนนบอส 1000
    ScoreManage.Instance?.AddScore(1000);

    // ปิด collider / physics / effect
    foreach (var c in GetComponentsInChildren<Collider2D>())
        c.enabled = false;

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

    if (glowEffect != null)
        glowEffect.SetActive(false);

    if (bossAnimator != null)
        bossAnimator.SetTrigger("Die");

    // ❌ ไม่ Destroy ทันที
    StartCoroutine(FinishBossDeath(2f));
}

private IEnumerator FinishBossDeath(float delay)
{
    yield return new WaitForSeconds(delay);

    // คืนการควบคุม player / map
    UnfreezeAllMapObjects();

    // แจ้ง MonsterManager
    if (MonsterManage.Instance != null)
        MonsterManage.Instance.EnemyKilled();

    // ลบตัวบอส
    Destroy(gameObject);
}


public void ResetBossState()
{
    if (state == BossState.Dead)
    {
        // รี spawn boss
        state = BossState.Normal;
        gameObject.SetActive(true);

        // รีเซ็ต health
        currentHealth = maxHealth;
        UpdateHealthBar();

        // เปิด collider และ physics
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (var c in cols)
            c.enabled = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;

        if (glowEffect != null)
            glowEffect.SetActive(false);

        if (bossAnimator != null)
            bossAnimator.Rebind();
    }
}
public static void ForceUnfreezeMap()
{
    Rigidbody2D[] bodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

    foreach (var rb in bodies)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // เปิด PlayerMovement ด้วย
    PlayerMovement pm = Object.FindFirstObjectByType<PlayerMovement>();
    if (pm != null) pm.enabled = true;
}
public void ResetBoss()
{
    // ถ้าบอสเคยตาย → ต้องเปิดใหม่
    gameObject.SetActive(true);

    // รีสถานะ
    state = BossState.Normal;

    // รีเลือด
    currentHealth = maxHealth;
    UpdateHealthBar();

    // ปิด glow
    if (glowEffect != null)
        glowEffect.SetActive(false);

    // เปิด collider
    Collider2D[] cols = GetComponentsInChildren<Collider2D>();
    foreach (var c in cols)
        c.enabled = true;

    // เปิด Rigidbody
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
        rb.bodyType = RigidbodyType2D.Dynamic;

    // รีอนิเมชัน
    if (bossAnimator != null)
        bossAnimator.Rebind();

    Debug.Log("Boss reset completed.");
}





}
