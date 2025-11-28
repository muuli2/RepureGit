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

private void Awake()  
{  
    Instance = this;  
    currentHealth = maxHealth;  
    UpdateHealthBar();  
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

    if (glowEffect != null) glowEffect.SetActive(true);
    if (bossAnimator != null) bossAnimator.SetTrigger("PhaseTransition");

    // แสดงข้อความ Intro
    IntroMinigame tt = UnityEngine.Object.FindFirstObjectByType<IntroMinigame>();
    if (tt != null)
        yield return tt.ShowText("SHOWDOWN");

    Debug.Log("Boss is preparing minigame...");
    yield return new WaitForSeconds(1f);

    // โหลดมินิเกมแบบ Additive
    SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
}


// เรียกหลังเล่นมินิเกมสำเร็จ  
public void BossDefeated()  
{  
    state = BossState.Dead;  

    // เล่นอนิเมชันสลายตัว  
    if(bossAnimator != null) bossAnimator.SetTrigger("Die");  

    // ลบบอสหลังอนิเมชันจบ (สมมติ 2 วินาที)  
    Destroy(gameObject, 2f);  
}  


}
