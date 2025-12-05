using UnityEngine;

public class HeartItem : MonoBehaviour
{
    public int healAmount = 1;       // จำนวนหัวใจที่เพิ่มให้ผู้เล่น
    public float lifetime = 10f;     // ไอเท็มหายหลัง 10 วิ

    private void Start()
    {
        // ทำให้ไอเท็มหายเองหลังหมดเวลา
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เพิ่มเลือดผู้เล่น
            GameManager.Instance.lives += healAmount;
            GameManager.Instance.lives = Mathf.Clamp(GameManager.Instance.lives, 0, GameManager.Instance.maxLives);
            GameManager.Instance.UpdateHeartsUI();

            // ทำลายไอเท็ม
            Destroy(gameObject);
        }
    }
}
