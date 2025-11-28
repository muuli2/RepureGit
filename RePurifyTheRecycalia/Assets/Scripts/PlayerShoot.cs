using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;        // จุดเกิดกระสุน
    public GameObject bulletPrefab;    // prefab กระสุน
    public float bulletSpeed = 10f;
    public float shootCooldown = 3f;   // เวลา cooldown
    private float lastShootTime = -Mathf.Infinity;

    void Update()
    {
        // ตรวจสอบว่า cooldown ครบและกดเมาส์
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;  // รีเซ็ตเวลา cooldown
        }
    }

    void Shoot()
    {
        // ตำแหน่งเมาส์ในโลก
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        // ทิศทางจาก firePoint ไปเมาส์
        Vector3 direction = (mousePos - firePoint.position).normalized;

        // สร้างกระสุน
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // หมุนกระสุนให้หันไปทางเมาส์
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ขับกระสุน
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}
