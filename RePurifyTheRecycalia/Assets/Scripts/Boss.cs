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
        {
            float fillPercent = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillPercent;
        }
    }

    // -------------------------------------
    // ⚔️ Start Minigame Transition
    // -------------------------------------

    private IEnumerator TriggerMinigameTransition()
    {
        state = BossState.WaitingMinigame;

        FreezeAllMapObjects();

        if (glowEffect != null) glowEffect.SetActive(true);
        if (bossAnimator != null) bossAnimator.SetTrigger("PhaseTransition");

        // UI Text "SHOWDOWN"
        IntroMinigame tt = Object.FindFirstObjectByType<IntroMinigame>();
        if (tt != null)
            yield return tt.ShowText("SHOWDOWN");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }

    // -------------------------------------
    // 💀 Boss Died After Minigame
    // -------------------------------------

    public void BossDefeated()
{
    if (state == BossState.Dead) return;
    state = BossState.Dead;

    if (bossAnimator != null)
        bossAnimator.SetTrigger("Die");

    // ปิด collider
    Collider2D[] cols = GetComponentsInChildren<Collider2D>();
    foreach (var c in cols)
        c.enabled = false;

    // ปิด Rigidbody2D
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // ปิด glow
    if (glowEffect != null)
        glowEffect.SetActive(false);

    // รอให้ scene กลับมาจากมินิเกมก่อนค่อยลบ
    StartCoroutine(DestroyAfterReturn(2f));
}

private IEnumerator DestroyAfterReturn(float delay)
{
    // รอให้อนิเมชัน Die เล่น
    yield return new WaitForSeconds(delay);

    // คืน Player / map control
    UnfreezeAllMapObjects();

    // แจ้ง MonsterManage
    if (MonsterManage.Instance != null)
        MonsterManage.Instance.EnemyKilled();

    // ลบตัวบอส
    Destroy(gameObject);
}

private IEnumerator FinishBossDeath(float delay)
{
    yield return new WaitForSeconds(delay);

    // คืนการควบคุม player และ object อื่น ๆ
    UnfreezeAllMapObjects();

    // แจ้ง MonsterManager ว่าศัตรูตายแล้ว
    if (MonsterManage.Instance != null)
        MonsterManage.Instance.EnemyKilled();

    // ลบ object ออกจาก scene
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
