using UnityEngine;

public class TrashItem2 : MonoBehaviour
{
    public enum TrashType2 { Wet, Dry, Recycle, Hazard }
    public TrashType2 trashType2;

    void Update()
    {
        // ตรวจสอบว่าขยะตกพ้นจอด้านล่าง
        if (transform.position.y < -6f) // ปรับค่า y ตามขอบจอ
        {
            // ถ้าขยะถูกชนิด จะถือว่าพลาด → ลดหัวใจ
            if (trashType2 == MiniGame012.Instance.targetTrashType)
            {
                GameManager.Instance.TakeDamage(1);
                MiniGame012.Instance.UpdateHeartsUI();

                if (GameManager.Instance.lives <= 0)
                    MiniGame012.Instance.GameOver();
            }

            Destroy(gameObject);
        }
    }
}
