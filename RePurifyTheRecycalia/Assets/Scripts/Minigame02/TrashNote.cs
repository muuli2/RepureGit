using UnityEngine;
using UnityEngine.InputSystem;

public class TrashNote : MonoBehaviour
{
    public enum TrashType { General, Wet, Recycle }

    public TrashType trashType;      // ชนิดขยะ
    public Key correctKey;           // ปุ่มของเลน
    public float speed = 1f;
    public float hitY = -1.5f;
    public float hitRange = 1.5f;

    private bool hit = false;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // ถ้าพ้น hit zone
        if (!hit && transform.position.y <= hitY - hitRange)
        {
            hit = true;
            RhythmMiniGame.Instance.LoseLife();
            Destroy(gameObject);
        }
    }

    // ทุกขยะสามารถกดได้หมด แต่คะแนนเฉพาะเป้าหมาย
    public void TryHit(Key key, TrashType targetType)
    {
        if (hit) return;
        hit = true;

        if (key == correctKey)
        {
            if (trashType == targetType)
            {
                RhythmMiniGame.Instance.AddScore(250); // ได้คะแนน
            }
            else
            {
                RhythmMiniGame.Instance.LoseLife();    // ลดหัวใจ
            }
        }
        else
        {
            RhythmMiniGame.Instance.LoseLife();        // ปุ่มผิด → ลดหัวใจ
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3(transform.position.x, hitY, transform.position.z);
        Gizmos.DrawWireSphere(center, hitRange);
    }
#endif
}
