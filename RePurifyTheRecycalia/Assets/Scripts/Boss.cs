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
    state = BossState.Dead;

    if (bossAnimator != null)
        bossAnimator.SetTrigger("Die");

    // ปิดฟิสิกส์
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;  // หยุดทุกฟิสิกส์
    }

    // ปิดคอลลิเดอร์ทั้งหมด
    Collider2D[] cols = GetComponentsInChildren<Collider2D>();
    foreach (var c in cols)
        c.enabled = false;

    UnfreezeAllMapObjects();

    // ทำลายหลังจากอนิเมชันจบ
    Destroy(gameObject, 2f);
    MonsterManage.Instance.EnemyKilled();
}

}
