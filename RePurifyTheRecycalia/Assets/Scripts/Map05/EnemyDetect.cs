using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public float detectRadius = 5f;    // ระยะที่บอสมองเห็นผู้เล่น
    public LayerMask playerLayer;      // เลือก layer Player
    private Transform targetPlayer;

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        // เช็คว่ายังมีผู้เล่นในรัศมีไหม
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);

        if (hit != null)
        {
            targetPlayer = hit.transform;
            // ให้บอสรู้ว่าผู้เล่นเข้าวงแล้ว
            Debug.Log("เจอผู้เล่น! ไล่ได้");
        }
        else
        {
            targetPlayer = null;
        }
    }

    // วาดวงใน Scene View ให้เห็นชัดๆ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
