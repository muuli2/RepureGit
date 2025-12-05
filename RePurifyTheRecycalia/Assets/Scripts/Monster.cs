using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DropData
{
    public GameObject item;
    public float dropChance; // 0-100%
}

public class Monster : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    public Image healthBarFill;

    [Header("Drop Settings")]
    public DropData[] drops;

    [Header("Attack Settings")]
    public int damageToPlayer = 1;      
    public float attackCooldown = 1f;   
    private float lastAttackTime = 0f;

    [Header("Score Settings")]
    public int scoreOnDeath = 150;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
{
    // สุ่มดรอปไอเท็ม
    foreach (var d in drops)
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= d.dropChance)
            Instantiate(d.item, transform.position, Quaternion.identity);
    }

    // เพิ่มคะแนน
    if (ScoreManage.Instance != null)
        ScoreManage.Instance.AddScore(scoreOnDeath); // ใช้ค่าที่ตั้งไว้ใน Inspector

    // ปิดมอนสเตอร์
    gameObject.SetActive(false);
}



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamagePlayer();
        }
    }

    void TryDamagePlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            GameManager.Instance.TakeDamage(damageToPlayer);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        gameObject.SetActive(true);
    }
}
