using UnityEngine;
using UnityEngine.InputSystem;

public class TrashNote : MonoBehaviour
{
    public enum TrashType { General, Wet, Recycle }

    public TrashType trashType;      
    public Key correctKey;           
    public float speed = 1f;

    // เขตที่ถือว่า "กดโดน"
    public float hitY = -1.5f;
    public float hitRange = 0.35f;

    private bool processed = false; 

    private float missY => hitY - hitRange - 0.15f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // ถ้าตกผ่าน missY → ถือว่า Miss
        if (!processed && transform.position.y <= missY)
        {
            processed = true;
            OnMiss();
        }
    }

    public bool IsInHitZone()
    {
        float y = transform.position.y;
        return (y <= hitY + hitRange) && (y >= hitY - hitRange);
    }

    // ถูกเรียกตอนผู้เล่นกดปุ่ม
    public void TryHit(Key key, TrashType targetType)
    {
        if (processed) return;

        // ปุ่มถูกต้องหรือไม่ (เลนเดียวกัน)
        if (key != correctKey) return;

        // ถ้าไม่ได้อยู่ในโซนกด ไม่ถือว่าโดน
        if (!IsInHitZone()) return;

        processed = true;

        // ------------ กดโดน ------------
        if (trashType == targetType)
        {
            // ✔ ถูกชนิด → ได้แต้ม
            RhythmMiniGame.Instance.AddScore(250);
        }
        else
        {
            // ❌ ผิดชนิด → ไม่ได้แต้ม และโดนลดใจ
            RhythmMiniGame.Instance.LoseLife();
        }

        Destroy(gameObject);
    }

    // ------------ ปล่อยตก (Miss) ------------
    private void OnMiss()
    {
        // ถูกชนิด → ลดใจ
        if (trashType == RhythmMiniGame.Instance.targetTrashType)
        {
            RhythmMiniGame.Instance.LoseLife();
        }
        // ผิดชนิด → ปล่อยตกได้ ไม่ลดใจ

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, hitY, transform.position.z), hitRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, missY, transform.position.z), 0.05f);
    }
#endif
}
