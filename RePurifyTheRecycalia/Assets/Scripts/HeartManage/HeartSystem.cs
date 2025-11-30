using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    public static HeartSystem Instance;

    [Header("Lives Settings")]
    public int maxLives = 5;          // จำนวนหัวใจสูงสุด
    public int currentLives;          // เลือดปัจจุบัน
    public Image[] heartImages;       // ลากหัวใจ UI 5 รูป
    public Sprite heartFull;          // หัวใจเต็ม
    public Sprite heartEmpty;         // หัวใจว่าง

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentLives = maxLives;
        UpdateHeartsUI();
    }

    // เรียกเพื่อลดเลือด
    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        UpdateHeartsUI();

        if (currentLives <= 0)
            PlayerDied();
    }

    // เรียกเพื่อเพิ่มเลือด
    public void Heal(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        UpdateHeartsUI();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
                heartImages[i].sprite = heartFull;
            else
                heartImages[i].sprite = heartEmpty;
        }
    }

    void PlayerDied()
    {
        Debug.Log("Game Over – ผู้เล่นหัวใจหมดแล้ว!");
        // เพิ่ม UI GameOver / Restart Scene ได้ตามต้องการ
    }
}
