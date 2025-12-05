using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManage : MonoBehaviour
{
    public static ScoreManage Instance;
    public TMP_Text scoreText;

    private int score = 0;
    public int totalScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var textObj = GameObject.Find("ScoreText");
        if (textObj != null)
            scoreText = textObj.GetComponent<TMP_Text>();

        // รีคะแนนเฉพาะแมพหลัก
        if (scene.name.StartsWith("Map"))
            ResetMapScore();

        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        totalScore += amount;
        UpdateScoreUI();
    }

    public void ResetMapScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
