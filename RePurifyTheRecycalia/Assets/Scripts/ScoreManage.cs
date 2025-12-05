using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManage : MonoBehaviour
{
    public static ScoreManage Instance; // ✅ ต้อง public static
    public TMP_Text scoreText;

    private int score = 0;               // คะแนนในด่าน (เช่นเก็บขยะ)
    private int scoreAtCheckpoint = 0;   // คะแนนที่บันทึกตอน checkpoint
    public int totalScore = 0;           // คะแนนรวม ใช้โชว์บน UI

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // เก็บไว้ตลอดเกม
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // รีหา Text ของ Canvas ใหม่ในซีนปัจจุบัน
        var textObj = GameObject.Find("ScoreText"); // ชื่อต้องตรงกับ Canvas
        if (textObj != null)
            scoreText = textObj.GetComponent<TMP_Text>();

        // รี UI ให้ตรงกับคะแนนรวม
        UpdateScoreUI();

        // รีคะแนนด่านใหม่
        score = 0;
        scoreAtCheckpoint = 0;
    }

    // อัปเดต UI คะแนนรวม
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = totalScore.ToString();
    }

    // อัปเดต UI คะแนนในด่าน (เช่นเก็บขยะ)
    void UpdateInMapScore()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public int GetScore() => score;

    // บันทึกคะแนนตอนถึง checkpoint
    public void SaveScoreAtCheckpoint()
    {
        scoreAtCheckpoint = score;
    }

    // รีคะแนนหลัง checkpoint
    public void ResetScoreAfterCheckpoint()
    {
        score = scoreAtCheckpoint;
        UpdateInMapScore();
    }

    // เพิ่ม/ลดคะแนนรวม
    public void AddScore(int amount)
    {
        totalScore += amount;
        if (totalScore < 0)
            totalScore = 0;

        UpdateScoreUI();
    }

    // เพิ่มคะแนนเฉพาะด่าน
    public void AddScoreInMap(int amount)
    {
        score += amount;
        if (score < 0)
            score = 0;

        UpdateInMapScore();
    }
}
