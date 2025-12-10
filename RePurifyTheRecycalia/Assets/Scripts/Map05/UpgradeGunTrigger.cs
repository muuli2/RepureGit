using UnityEngine;

public class UpgradeGunTrigger : MonoBehaviour
{
    // private bool hasUpgraded = false;   // กันไม่ให้กินทริกเกอร์รัว ๆ

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (hasUpgraded) return;  // อัพไปแล้วก็ไม่ต้องอัพซ้ำ

        if (other.CompareTag("Player"))
        {
            PlayerShoot ps = other.GetComponent<PlayerShoot>();
            if (ps != null)
            {
                ps.UpgradeGun();
                // hasUpgraded = true;
            }

            // ถ้าอยากให้ Trigger ทำงานได้เรื่อย ๆ (อัพซ้ำได้)
            // ก็ลบ hasUpgraded = true; ทิ้ง
        }
    }
}
