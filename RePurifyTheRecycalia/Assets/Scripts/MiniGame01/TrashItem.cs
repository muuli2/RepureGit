using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public enum TrashType { Wet, Dry, Recycle, Hazard }
    public TrashType trashType;

    void Update()
    {
        // ตรวจสอบว่าขยะตกพ้นจอด้านล่าง
        if (transform.position.y < -6f) // ปรับค่า y ตามขอบจอ
        {
            // ถ้าขยะถูกชนิด จะถือว่าพลาด → ลดหัวใจ
            if (trashType == MiniGame01.Instance.targetTrashType)
            {
                GameManager.Instance.TakeDamage(1);
                MiniGame01.Instance.UpdateHeartsUI();

                if (GameManager.Instance.lives <= 0)
                    MiniGame01.Instance.GameOver();
            }

            Destroy(gameObject);
        }
    }
}
