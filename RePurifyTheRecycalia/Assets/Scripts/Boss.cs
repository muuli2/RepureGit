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
    public AudioSource bgmSource;

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
    // ❄ Freeze / Unfreeze Map & Player
    // -------------------------------------
    // Boss.cs
void FreezeMapAndPlayer()
{
    Scene mapScene = SceneManager.GetSceneByName("Map01");
    if (!mapScene.isLoaded) return;

    // Freeze ทั้งแมพ
    GameObject[] rootObjects = mapScene.GetRootGameObjects();
    foreach (var obj in rootObjects)
    {
        foreach (var rb in obj.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Freeze Player ทุกตัวที่มี Tag = "Player"
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (var player in players)
    {
        if (player.scene.name == "Map01")
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}

public void UnfreezeMapAndPlayer()
{
    // Unfreeze Rigidbody ทั้งแมพ
    foreach (var rb in Object.FindObjectsOfType<Rigidbody2D>())
    {
        if (rb.gameObject.scene.name == "Map01")
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Unfreeze Player ทุกตัว
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (var player in players)
    {
        if (player.scene.name == "Map01")
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = true;
        }
    }
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
            StartCoroutine(TriggerMinigameTransition());
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    // -------------------------------------
    // 🔹 Trigger Minigame
    // -------------------------------------
    private IEnumerator TriggerMinigameTransition()
    {

                       
        state = BossState.WaitingMinigame;

        // Freeze everything at current position
        FreezeMapAndPlayer();

        if (glowEffect != null) glowEffect.SetActive(true);
        if (bossAnimator != null) bossAnimator.SetTrigger("PhaseTransition");

        // แสดง SHOWDOWN ขณะ freeze (ใช้ Realtime)
        IntroMinigame tt = Object.FindFirstObjectByType<IntroMinigame>();
        if (tt != null)
            yield return tt.ShowText("SHOWDOWN");

        // รอเล็กน้อยเพื่อให้ player เห็น effect
        yield return new WaitForSecondsRealtime(0.5f);

        // บอก GameManager ว่าเป็นมินิเกม
        if (GameManager.Instance != null)
            GameManager.Instance.isMiniGameActive = true;

        // โหลดมินิเกม Additive
        bgmSource.Stop();

        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);

        
    }

    // -------------------------------------
    // 💀 Boss Defeated
    // -------------------------------------
    public void BossDefeated()
    {
        if (state == BossState.Dead) return;
        state = BossState.Dead;

        ScoreManage.Instance?.AddScore(1000);
         bgmSource.Play();


        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        if (glowEffect != null) glowEffect.SetActive(false);
        if (bossAnimator != null) bossAnimator.SetTrigger("Die");

        StartCoroutine(FinishBossDeath(2f));
    }

    private IEnumerator FinishBossDeath(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        UnfreezeMapAndPlayer();

        if (MonsterManage.Instance != null)
            MonsterManage.Instance.EnemyKilled();

        Destroy(gameObject);
    }

    // -------------------------------------
    // 🔄 Reset Boss
    // -------------------------------------
    public void ResetBossState()
    {
        if (state == BossState.Dead)
        {
            state = BossState.Normal;
            gameObject.SetActive(true);
            currentHealth = maxHealth;
            UpdateHealthBar();

            foreach (var c in GetComponentsInChildren<Collider2D>())
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

    // public void ResetBoss()
    // {
    //     gameObject.SetActive(true);
    //     state = BossState.Normal;
    //     currentHealth = maxHealth;
    //     UpdateHealthBar();

    //     foreach (var c in GetComponentsInChildren<Collider2D>())
    //         c.enabled = true;

    //     Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //     if (rb != null)
    //         rb.bodyType = RigidbodyType2D.Dynamic;

    //     if (glowEffect != null)
    //         glowEffect.SetActive(false);
    //     if (bossAnimator != null)
    //         bossAnimator.Rebind();

    //     Debug.Log("Boss reset completed.");
    // }

    // -------------------------------------
    // 🔹 Helper Static
    // -------------------------------------
    public static void ForceUnfreezeMap()
    {
        Rigidbody2D[] bodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);
        foreach (var rb in bodies)
            rb.bodyType = RigidbodyType2D.Dynamic;

        PlayerMovement pm = Object.FindFirstObjectByType<PlayerMovement>();
        if (pm != null)
            pm.enabled = true;

        Time.timeScale = 1f;
    }

}