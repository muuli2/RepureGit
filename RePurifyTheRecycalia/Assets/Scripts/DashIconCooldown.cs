using UnityEngine;
using UnityEngine.UI;

public class DashIconCooldown : MonoBehaviour
{
    public Image dashIcon;
    public PlayerMovement player;  // อ้าง PlayerMovement

    private bool isOnCooldown = false;

    void Update()
    {
        // ถ้า Player ยังไม่ตั้งคูลดาวน์ ก็อย่าสลับสถานะ
        if (player == null) return;

        // ถ้าแดชอยู่ในคูลดาวน์
        if (player.dashCDTimer > 0)
        {
            if (!isOnCooldown)
            {
                SetIconAlpha(0.5f); // ทำให้ใสลง
                isOnCooldown = true;
            }
        }
        else
        {
            // คูลดาวน์จบ
            if (isOnCooldown)
            {
                SetIconAlpha(1f); // กลับมาเข้มเต็ม
                isOnCooldown = false;
            }
        }
    }

    void SetIconAlpha(float a)
    {
        Color c = dashIcon.color;
        c.a = a;
        dashIcon.color = c;
    }
}
