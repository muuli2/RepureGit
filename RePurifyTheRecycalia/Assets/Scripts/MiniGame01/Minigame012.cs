using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGame012 : MonoBehaviour
{
    public static MiniGame012 Instance;

    public int targetScore = 1000;
    public int score = 0;
    public bool gameStarted = false;

    [Header("UI")]
    public TMP_Text scoreText;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    public string mapSceneName = "Map02";

    // ✅ แก้ enum ให้ถูกชนิดเดียวกับ TrashItem2
    public TrashItem2.TrashType2 targetTrashType = TrashItem2.TrashType2.Wet;

    private void Awake() { Instance = this; }

    private void Start()
    {
        PauseManager pause = Object.FindFirstObjectByType<PauseManager>();

        if (pause != null)
            pause.isMiniGameActive = true;

        UpdateHeartsUI();
        UpdateScoreUI();
        gameStarted = true;

        gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score >= targetScore)
        {
            WinGame();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateHeartsUI()
    {
        int currentLives = GameManager.Instance.lives;
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentLives ? heartFull : heartEmpty;
        }
    }

    // ✅ แก้ CollectTrash ให้ใช้ TrashItem2 (ไม่ใช่ TrashItem)
    public void CollectTrash(TrashItem2 trash)
    {
        int points = 1000;

        if (trash.trashType2 == targetTrashType)
        {
            AddScore(points);
        }
        else
        {
            GameManager.Instance.TakeDamage(1);
            UpdateHeartsUI();
        }

        Destroy(trash.gameObject);

        if (GameManager.Instance.lives <= 0)
        {
            SceneManager.UnloadSceneAsync("MiniGame012");
            GameManager.Instance.PlayerDied();
        }
    }

    void DestroyAllTrash()
    {
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        foreach (var trash in trashObjects)
            Destroy(trash);
    }

    void FreezeAllTrash()
    {
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        foreach (var trash in trashObjects)
        {
            var rb = trash.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        var spawner = Object.FindFirstObjectByType<TrashSpawner>();
        if (spawner != null)
            spawner.StopSpawn();

        DestroyAllTrash();
        Time.timeScale = 0;
    }

    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);

        var spawner = Object.FindFirstObjectByType<TrashSpawner>();
        if (spawner != null)
            spawner.StopSpawn();

        FreezeAllTrash();
        DestroyAllTrash();

        Time.timeScale = 0;
    }

    public void ContinueToMap()
    {
        Time.timeScale = 1;
        if (winPanel != null)
            winPanel.SetActive(false);

        SceneManager.UnloadSceneAsync("MiniGame012").completed += (op) =>
        {
            if (Boss.Instance != null && Boss.Instance.state != Boss.BossState.Dead)
            {
                Boss.Instance.BossDefeated();
            }

            PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
            if (pause != null)
                pause.isMiniGameActive = false;
        };
    }

    public void ReplayMinigame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MiniGame012", LoadSceneMode.Additive);
    }

    public void RetryMap()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Map02");
    }
}
