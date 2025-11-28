using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DropData
{
    public GameObject item;      // ไอเท็มที่จะดรอป
    public float dropChance;     // โอกาสดรอป (0 - 100%)
}

public class Monster : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    public Image healthBarFill;

    [Header("Drop Settings")]
    public DropData[] drops;   // << ใช้อันนี้แทน dropItems

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
        foreach (var d in drops)
        {
            float roll = Random.Range(0f, 100f);

            // ถ้าสุ่มได้ตามเปอร์เซ็นต์ ก็จะดรอป
            if (roll <= d.dropChance)
            {
                Instantiate(d.item, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
