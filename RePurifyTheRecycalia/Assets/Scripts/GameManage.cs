using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
public static GameManager Instance;


[Header("Player Prefabs")]
public GameObject KnightPrefab;
public GameObject MagePrefab;
public Transform spawnPoint;

[Header("Lives System")]
public int lives = 5;                     // จำนวนหัวใจเริ่มต้น
public Image[] heartImages;               // UI รูปหัวใจ 5 ดวง
public Sprite heartFull;
public Sprite heartEmpty;

[Header("Game Over UI")]
public GameObject gameOverPanel;          // Panel Game Over
public Button restartButton;              // ปุ่ม Restart

private GameObject playerRef;             // reference player ที่ spawn ออกมา

private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
}

private void Start()
{
    SpawnPlayer();
    UpdateHeartsUI();

    // ปิด panel ตอนเริ่ม
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    // เพิ่ม listener ให้ปุ่ม Restart
    if (restartButton != null)
        restartButton.onClick.AddListener(RestartGame);
}

// -------------------------------------------------------
// Spawn Player ตามตัวที่เลือก
// -------------------------------------------------------
void SpawnPlayer()
{
    GameObject toSpawn = null;

    if (SelectedCharacter.characterName == "Knight")
        toSpawn = KnightPrefab;
    else if (SelectedCharacter.characterName == "Mage")
        toSpawn = MagePrefab;

    if (toSpawn != null)
    {
        playerRef = Instantiate(toSpawn, spawnPoint.position, Quaternion.identity);

        // Add PlayerInput ถ้าไม่มี
        if (playerRef.GetComponent<PlayerInput>() == null)
            playerRef.AddComponent<PlayerInput>();

        // Assign Rigidbody2D ให้ PlayerMovement ถ้าใช้อยู่
        PlayerMovement movement = playerRef.GetComponent<PlayerMovement>();
        if (movement != null && movement.rb == null)
            movement.rb = playerRef.GetComponent<Rigidbody2D>();

        // กล้องตาม player
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.target = playerRef.transform;
    }
    else
    {
        Debug.LogWarning("Prefab ไม่ถูกตั้งค่า หรือ characterName ผิด!");
    }
}

// -------------------------------------------------------
// ระบบหัวใจ
// -------------------------------------------------------
public void TakeDamage(int amount)
{
    lives -= amount;
    lives = Mathf.Clamp(lives, 0, 5);

    UpdateHeartsUI();

    if (lives <= 0)
        PlayerDied();
}

void UpdateHeartsUI()
{
    for (int i = 0; i < heartImages.Length; i++)
    {
        heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
    }
}



// -------------------------------------------------------
// Player ตาย / Game Over
// -------------------------------------------------------
void PlayerDied()
{
    Debug.Log("Game Over – ผู้เล่นหัวใจหมดแล้ว!");
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    // ปิด Player
    if (playerRef != null)
        playerRef.SetActive(false);
}

void RestartGame()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


}
