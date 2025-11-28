using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGame01 : MonoBehaviour
{
    public static MiniGame01 Instance;
    

    public int lives = 5;
    public int score = 0;
    public int targetScore = 5000;
    public bool gameStarted = false;

    [Header("UI")]
    public TMP_Text scoreText;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    public string mapSceneName = "Map01"; // ซีนแมพหลัก
    public TrashItem.TrashType targetTrashType = TrashItem.TrashType.Wet;

    private void Awake() { Instance = this; }

    private void Start()
    {
        UpdateHeartsUI();
        UpdateScoreUI();
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

    public void LoseLife()
    {
        lives--;
        UpdateHeartsUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // ปุ่ม Win Panel: ไปต่อ (กลับ Map01)
    public void ContinueToMap()
    {
        Time.timeScale = 1;
        if (winPanel != null)
            winPanel.SetActive(false);

        // แจ้ง Boss ว่า minigame ผ่าน
        if (Boss.Instance != null)
            Boss.Instance.BossDefeated();

        

        // Unload มินิเกม Scene (Additive)
        SceneManager.UnloadSceneAsync("MiniGame01");

        
    }

    // ปุ่ม Win Panel: เล่นมินิเกมใหม่
    public void ReplayMinigame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MiniGame01", LoadSceneMode.Additive);
    }

    // ปุ่ม GameOver Panel: เล่นใหม่
    public void RetryMinigame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MiniGame01", LoadSceneMode.Additive);
    }

    // ปุ่ม GameOver Panel: รีด่าน (กลับแมพ)
    public void RetryMap()
    {
        Time.timeScale = 1;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapSceneName));
    }
}
