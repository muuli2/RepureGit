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
    public int maxLives = 5;
    public int lives;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button restartButton;

    [HideInInspector]
    public Vector3 lastCheckpoint;

    

    private GameObject playerRef;
    public bool isMiniGameActive = false;

    public int mapPoints = 0; // คะแนนในแมพ

    public GameObject GetPlayer() => playerRef;

    private void Awake()
    {
        Instance = this;
        lives = maxLives;  // รีหัวใจเต็ม
        mapPoints = 0;     // รีคะแนนแมพ
    }

    private void Start()
    {
        UpdateHeartsUI();
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartFromGameOver);

        RestartFromScene();
    }

    public void RestartFromScene()
{
    // รีหัวใจเต็ม
    lives = maxLives;
    UpdateHeartsUI();

    // รี GameOver Panel
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    // รี Pause Menu ถ้ามี
    PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
    if (pause != null)
        pause.ResetPauseMenu();

    // รีคะแนนแมพ
    ScoreManage.Instance?.ResetMapScore();

    // รี spawn player
    Vector3 spawnPos = lastCheckpoint != Vector3.zero ? lastCheckpoint : spawnPoint.position;
    SpawnPlayer(spawnPos);
}


    public void SpawnPlayer(Vector3 position)
    {
        if (playerRef != null)
        {
            playerRef.SetActive(true);
            playerRef.transform.position = position;

            var movement = playerRef.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = true;

            var rb = playerRef.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            return;
        }

        GameObject toSpawn = null;
if (SelectedCharacter.characterName == "Knight")
    toSpawn = KnightPrefab;
else if (SelectedCharacter.characterName == "Lumina")
    toSpawn = MagePrefab;

if (toSpawn != null)
{
    playerRef = Instantiate(toSpawn, position, Quaternion.identity);

    // ✅ เพิ่ม PlayerInput ถ้าไม่มี
    if (playerRef.GetComponent<PlayerInput>() == null)
        playerRef.AddComponent<PlayerInput>();

    // ✅ เปิด PlayerMovement และ Rigidbody
    var movement = playerRef.GetComponent<PlayerMovement>();
    if (movement != null)
    {
        movement.enabled = true;
        if (movement.rb == null)
            movement.rb = playerRef.GetComponent<Rigidbody2D>();
    }

    var rb = playerRef.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    // ✅ ให้กล้องตาม player ใหม่
    var camFollow = Camera.main.GetComponent<CameraFollow>();
    if (camFollow != null)
        camFollow.target = playerRef.transform;
}

        else
        {
            Debug.LogWarning("Prefab ไม่ถูกตั้งค่า หรือ characterName ผิด!");
        }
    }

    public void TakeDamage(int amount)
    {
        lives -= amount;
        lives = Mathf.Clamp(lives, 0, maxLives);
        UpdateHeartsUI();

        if (lives <= 0)
            PlayerDied();
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
            heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
    }

    void PlayerDied()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (playerRef != null)
            playerRef.SetActive(false);
    }

    public void RestartFromGameOver()
    {
        ScoreManage.Instance?.ResetMapScore();
        SceneManager.LoadScene("Map01");
    }
}
