using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PauseManager pauseManager;


    [Header("Bullet Settings (Normal)")]
    public GameObject normalBulletPrefab;   // กระสุนธรรมดา
    public float normalBulletSpeed = 10f;

    [Header("Bullet Settings (Upgraded)")]
    public GameObject upgradedBulletPrefab; // กระสุนหลังอัพเกรด
    public float upgradedBulletSpeed = 18f; // ยิงแรงขึ้นหรือเร็วขึ้น

    [Header("Gun Upgrade")]
    public bool gunUpgraded = false;        // เริ่มต้นยังไม่อัพเกรด

    public float shootCooldown = 1f;
    private float lastShootTime = -Mathf.Infinity;

    [HideInInspector] public bool canShoot = true;

      void Start()
    {
         pauseManager = FindObjectOfType<PauseManager>();
    }

    void Update()
    {

        if (!canShoot || (pauseManager != null && (pauseManager.pauseMenu.activeSelf || pauseManager.confirmPanel.activeSelf)))
        return;
      

        if ((Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
            && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    void Shoot()
    {
        // หาทิศยิง
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;
        Vector3 direction = (mousePos - firePoint.position).normalized;

        // เลือก Prefab ตามอัพเกรด
        GameObject prefab = gunUpgraded ? upgradedBulletPrefab : normalBulletPrefab;
        float speed = gunUpgraded ? upgradedBulletSpeed : normalBulletSpeed;

        // สร้างกระสุน
        GameObject bullet = Instantiate(prefab, firePoint.position, Quaternion.identity);

        // หมุน
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ยิง
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
    }

    // ฟังก์ชันนี้จะถูกเรียกตอนคุยกับอาจารย์
    public void UpgradeGun()
    {
        gunUpgraded = true;
        Debug.Log("🔥 ปืนอัพเกรดแล้ว!");
    }
}
