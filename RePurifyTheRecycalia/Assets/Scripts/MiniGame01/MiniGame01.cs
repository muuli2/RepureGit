using UnityEngine;
using UnityEngine.UI;

public class MiniGame01 : MonoBehaviour
{
    public static MiniGame01 Instance;

    public int lives = 5;
    public int score = 0;
    public int targetScore = 5000;     // คะแนนเป้าหมาย

    public bool gameStarted = false;  // ตรวจสอบให้เป็น public


    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public GameObject gameOverPanel;
    public GameObject winPanel;        // Panel แสดงเมื่อครบคะแนน

    public TrashItem.TrashType targetTrashType = TrashItem.TrashType.Wet;

    private void Awake() { Instance = this; }

    private void Start()
    {
        UpdateHeartsUI();
        gameOverPanel.SetActive(false);
        if(winPanel != null) winPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;

        if(score >= targetScore)
        {
            WinGame();
        }
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
        if(winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
