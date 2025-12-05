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

    public GameObject GetPlayer()
{
    return playerRef;
}


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            lastCheckpoint = spawnPoint.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartFromGameOver);

        RestartFromScene();
    }

    public void RestartFromGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        RestartFromScene();
    }

    public void RestartFromPause()
{
    // รีเซ็ตเวลา
    Time.timeScale = 1f;

    // ปิด GameOver panel เผื่อเปิดอยู่
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    // ปิด Pause Menu ถ้ามี
    var pauseManager = FindObjectOfType<PauseManager>();
    if (pauseManager != null)
        pauseManager.ClosePauseMenu();

    RestartFromScene();
}


    private void RestartFromScene()
    {
        // รีเซ็ตหัวใจเต็ม
        lives = maxLives;
        UpdateHeartsUI();

        // ปิด panel ทุกอัน
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Vector3 spawnPos = lastCheckpoint != Vector3.zero ? lastCheckpoint : spawnPoint.position;
        SpawnPlayer(spawnPos);

        // รีเซ็ต checkpoint ทุกตัว
        foreach (var cp in FindObjectsOfType<Checkpoint>())
            cp.ResetCheckpoint();
    }

    public void SpawnPlayer(Vector3 position)
{
    if (playerRef != null)
    {
        playerRef.SetActive(true);
        playerRef.transform.position = position;

        // ✅ เปิดสคริปต์ PlayerMovement และ Rigidbody
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

        if (playerRef.GetComponent<PlayerInput>() == null)
            playerRef.AddComponent<PlayerInput>();

        var movement = playerRef.GetComponent<PlayerMovement>();
        if (movement != null && movement.rb == null)
            movement.rb = playerRef.GetComponent<Rigidbody2D>();

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
}
