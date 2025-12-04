using UnityEngine;
using TMPro;

public class ScoreManage : MonoBehaviour
{
    public static ScoreManage Instance;
    public TMP_Text scoreText;

    private int score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}
