using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PauseManager pauseManager;

    [Header("Bullet Settings (Normal)")]
    public GameObject normalBulletPrefab;
    public float normalBulletSpeed = 10f;
    public int normalBulletDamage = 10;  // ← เพิ่มดาเมจ

    [Header("Bullet Settings (Upgraded)")]
    public GameObject upgradedBulletPrefab;
    public float upgradedBulletSpeed = 18f;
    public int upgradedBulletDamage = 25; // ← เพิ่มดาเมจ

    [Header("Gun Upgrade")]
    public bool gunUpgraded = false;

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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;
        Vector3 direction = (mousePos - firePoint.position).normalized;

        // เลือก Prefab และค่าต่าง ๆ ตามอัพเกรด
        GameObject prefab = gunUpgraded ? upgradedBulletPrefab : normalBulletPrefab;
        float speed = gunUpgraded ? upgradedBulletSpeed : normalBulletSpeed;
        int damage = gunUpgraded ? upgradedBulletDamage : normalBulletDamage;

        GameObject bullet = Instantiate(prefab, firePoint.position, Quaternion.identity);

        // หมุนกระสุน
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ยิง
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;

        // ตั้งค่าดาเมจให้กระสุน (สมมติกระสุนมีสคริป Bullet)
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = damage;
        }
    }

    public void UpgradeGun()
    {
        gunUpgraded = true;
        Debug.Log("🔥 ปืนอัพเกรดแล้ว!");
    }
}
