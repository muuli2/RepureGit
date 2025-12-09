using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalMinigameManager : MonoBehaviour
{
    public static FinalMinigameManager Instance;

      [Header("Question Data")]
    public QuizQuestion[] questions;
    private int currentIndex = 0;

    [Header("UI")]
    public Image questionImage;
    public TMP_Text questionText;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text fastText;   // โชว์ข้อความ “ไวมาก!”
    public GameObject[] answerButtons;
     public GameObject winPanel;

    [Header("Start Panel")]
    public GameObject startPanel;
    public TMP_Text startCountdownText;

    [Header("Game Settings")]
    public float answerTimeLimit = 10f;
    public int baseScore = 10;
    public int bonusScore = 5;
    public int bonusLimitSeconds = 3;
    public int targetScore = 100;

    private float questionTimer;
    private bool isPlaying = false;
    private int score = 0;

    void Start()
    {
        fastText.gameObject.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        startPanel.SetActive(true);
        int count = 4;

        while (count > 0)
        {
            startCountdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        startPanel.SetActive(false);
        StartQuiz();
    }

    void StartQuiz()
    {
        score = 0;
        UpdateScoreUI();
        currentIndex = 0;
        ShowQuestion();
    }

    void Update()
    {
        if (isPlaying)
        {
            questionTimer -= Time.deltaTime;
            timerText.text = questionTimer.ToString("F1");

            if (questionTimer <= 0)
            {
                isPlaying = false;
                NextQuestion();
            }
        }
    }

    void ShowQuestion()
    {
        if (currentIndex >= questions.Length)
        {
            currentIndex = 0; // หรือสับใหม่ก็ได้
        }

        QuizQuestion q = questions[currentIndex];

        questionImage.sprite = q.picture;
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
            int index = i;
            answerButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            answerButtons[i].GetComponent<Button>().onClick.AddListener(() => OnAnswerClicked(index));
        }

        questionTimer = answerTimeLimit;
        isPlaying = true;
    }

    void OnAnswerClicked(int index)
    {
        isPlaying = false;

        QuizQuestion q = questions[currentIndex];

        if (index == q.correctIndex)
        {
            int gained = baseScore;

            // ได้โบนัสเพราะตอบไว
            if (questionTimer >= answerTimeLimit - bonusLimitSeconds)
            {
                gained += bonusScore;
                ShowFastText();
            }

            score += gained;
            UpdateScoreUI();

            if (score >= targetScore)
            {
                WinGame();
                return;
            }
        }

        NextQuestion();
    }

    void NextQuestion()
    {
        currentIndex++;
        ShowQuestion();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void ShowFastText()
    {
        StartCoroutine(FastTextRoutine());
    }

    IEnumerator FastTextRoutine()
    {
        fastText.text = "ไวมาก!";
        fastText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        fastText.gameObject.SetActive(false);
    }

    void WinGame()
    {
        isPlaying = false;
        timerText.text = "You Win!";
         winPanel.SetActive(true);
        // ใส่เปิดแพเนลหรือเปลี่ยนซีนตรงนี้ได้
    }

      public void ContinueToMap()
    {
        winPanel.SetActive(false);
        SceneManager.UnloadSceneAsync("MinigameFinal");
    }
}