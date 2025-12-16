using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    private PauseManager pauseManager;
     private Animator anim;
     

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
    
    [Header("Sound")]
public AudioClip shootSound;
private AudioSource audioSource;


    public float shootCooldown = 1f;
    private float lastShootTime = -Mathf.Infinity;

    [HideInInspector] public bool canShoot = true;

//     [Header("Disable Shoot In Scenes")]
// public string[] noShootScenes =
// {
//     "Minigame01",
//     "Minigame02",
//     "Minigame012",
//     "Minigame022",
//     "MiniGameFinal",
//     "MainMenu",
//     "CharacterSelect",
//     "CutScenes",
//     "End"
// };


    void Start()
{
    pauseManager = FindObjectOfType<PauseManager>();
    anim = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
}


   void Update()
{
    if (!canShoot) return;
    // if (IsNoShootScene()) return;
     if (GameManager.Instance != null && GameManager.Instance.isMiniGameActive)
        return;

    if (pauseManager != null &&
        (pauseManager.pauseMenu.activeSelf || pauseManager.confirmPanel.activeSelf))
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
    anim.SetTrigger("isAttack");

    if (shootSound != null)
        audioSource.PlayOneShot(shootSound);

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    mousePos.z = 0f;
    Vector3 direction = (mousePos - firePoint.position).normalized;

    GameObject prefab = gunUpgraded ? upgradedBulletPrefab : normalBulletPrefab;
    float speed = gunUpgraded ? upgradedBulletSpeed : normalBulletSpeed;
    int damage = gunUpgraded ? upgradedBulletDamage : normalBulletDamage;

    GameObject bullet = Instantiate(prefab, firePoint.position, Quaternion.identity);

    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    rb.linearVelocity = direction * speed;

    Bullet bulletScript = bullet.GetComponent<Bullet>();
    if (bulletScript != null)
        bulletScript.damage = damage;
}

// bool IsNoShootScene()
// {
//     for (int i = 0; i < SceneManager.sceneCount; i++)
//     {
//         string sceneName = SceneManager.GetSceneAt(i).name;
//         foreach (string s in noShootScenes)
//         {
//             if (sceneName == s)
//                 return true;
//         }
//     }
//     return false;
// }





    public void UpgradeGun()
    {
        gunUpgraded = true;
        Debug.Log("🔥 ปืนอัพเกรดแล้ว!");
    }
}
