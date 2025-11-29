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
    public Image healthBarFill;       // World Space HealthBar (Fill: Horizontal)  

    [Header("Effects")]  
    public Animator bossAnimator;     // Animator สำหรับทรานซิชัน  
    public GameObject glowEffect;     // ตัวอย่างแสงส่องบอส  

    [Header("Minigame")]  
    public string miniGameSceneName = "MiniGame01";  

   private PlayerMovement mapPlayerMovement;

    private void Awake()  
    {  
        Instance = this;  
        currentHealth = maxHealth;  
        UpdateHealthBar();  
    }  

    // --- Freeze / Unfreeze Map ---
   

void FreezeAllMapObjects()
{
    Rigidbody2D[] bodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

    foreach (var rb in bodies)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    // Freeze PlayerMovement script
    PlayerMovement player = FindObjectOfType<PlayerMovement>();
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

    // Unfreeze PlayerMovement script
    PlayerMovement player = FindObjectOfType<PlayerMovement>();
    if (player != null)
        player.enabled = true;
}




    public void TakeDamage(int damage)  
    {  
        if(state != BossState.Normal) return;  

        currentHealth -= damage;  
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  
        UpdateHealthBar();  

        if(currentHealth <= 0)  
        {  
            StartCoroutine(TriggerMinigameTransition());  
        }  
    }  

    private void UpdateHealthBar()  
    {  
        if(healthBarFill != null)  
        {  
            float fillPercent = (float)currentHealth / maxHealth;  
            healthBarFill.fillAmount = fillPercent;  
        }  
    }  

    private IEnumerator TriggerMinigameTransition()
    {
        state = BossState.WaitingMinigame;

        // Freeze ทุกอย่างในแมพ
        FreezeAllMapObjects();

        // แสดง Glow / Animation
        if (glowEffect != null) glowEffect.SetActive(true);
        if (bossAnimator != null) bossAnimator.SetTrigger("PhaseTransition");

        // Show Text "SHOWDOWN"
        IntroMinigame tt = UnityEngine.Object.FindFirstObjectByType<IntroMinigame>();
        if (tt != null)
            yield return tt.ShowText("SHOWDOWN");  // ข้อความวิ่งจากซ้ายไปขวา

        yield return new WaitForSeconds(1f); // เวลาชั่วคราวหลังข้อความ

        Debug.Log("Boss is preparing minigame...");

        // โหลดมินิเกมแบบ Additive
        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }

    // เรียกหลังเล่นมินิเกมสำเร็จ  
    // เรียกหลังเล่นมินิเกมสำเร็จ  
public void BossDefeated()  
{  
    state = BossState.Dead;  

    if(bossAnimator != null) bossAnimator.SetTrigger("Die");  

    // ยกเลิก freeze map + player
    UnfreezeAllMapObjects();

    Destroy(gameObject, 2f);  
}  

}
