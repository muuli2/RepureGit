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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        lives = maxLives;
    }

    private void Start()
    {
        RefreshUI();

        // ปิด GameOver Panel ตอนเริ่ม
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartFromGameOver);
    }

    public void RestartFromScene()
    {
        // รีหัวใจเต็ม
        lives = maxLives;
        UpdateHeartsUI();

        // ปิด GameOver Panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // รี Pause Menu
        PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
        if (pause != null)
            pause.ResetPauseMenu();

        // รีคะแนนแมพ
        ScoreManage.Instance?.ResetMapScore();

        // รี spawn player
        Vector3 spawnPos = lastCheckpoint != Vector3.zero ? lastCheckpoint :
                           (spawnPoint != null ? spawnPoint.position : Vector3.zero);
        SpawnPlayer(spawnPos);

        // รีมอนสเตอร์และขยะ
        MonsterManage.Instance?.ResetAllMonsters();
        TrashManage.Instance?.ResetAllTrash();

        // รีบอส
        Boss.Instance?.ResetBoss();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // เผื่อเกมถูก pause อยู่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnPlayer(Vector3 position)
    {
        if (playerRef != null)
            Destroy(playerRef);

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
        if (heartImages == null || heartImages.Length == 0)
            return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
        }
    }

    public void PlayerDied()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (playerRef != null)
            playerRef.SetActive(false);
    }

    public void RestartFromGameOver()
    {
        RestartFromScene();
    }

    public void ResetBossIfExists()
    {
        if (Boss.Instance != null)
            Boss.Instance.ResetBoss();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // รีหัวใจเต็ม
    lives = maxLives;

    // หา UI ใหม่ใน Scene
    RefreshUI();

    // ปิด GameOver Panel ตอนโหลด Scene
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    // หา spawnPoint ใหม่ถ้า null
    if (spawnPoint == null)
    {
        GameObject sp = GameObject.Find("Respawnpoint2"); // ตั้งชื่อ Object ใน Scene ว่า "PlayerSpawnPoint"
        if (sp != null)
            spawnPoint = sp.transform;
        else
            Debug.LogWarning("SpawnPoint ไม่พบใน Scene");
    }

    // รี spawn player
    Vector3 spawnPos = lastCheckpoint != Vector3.zero ? lastCheckpoint :
                       (spawnPoint != null ? spawnPoint.position : Vector3.zero);
    SpawnPlayer(spawnPos);

    // รีมอนสเตอร์และขยะ
    MonsterManage.Instance?.ResetAllMonsters();
    TrashManage.Instance?.ResetAllTrash();
    Boss.Instance?.ResetBoss();
}


    private void RefreshUI()
    {
        // หา Heart Images ใหม่
        GameObject heartContainer = GameObject.Find("HeartContainer");
        if (heartContainer != null)
            heartImages = heartContainer.GetComponentsInChildren<Image>();

        // หา GameOverPanel ใหม่
        if (gameOverPanel == null)
            gameOverPanel = GameObject.Find("GameOverPanel");

        UpdateHeartsUI();
    }
}
