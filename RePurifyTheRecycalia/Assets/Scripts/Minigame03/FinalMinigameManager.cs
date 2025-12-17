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
    public TMP_Text feedbackText; // โชว์ข้อความ “ผิดแล้ว!”
    public GameObject[] answerButtons;
    public GameObject winPanel;

    [Header("Start Panel")]
    public GameObject startPanel;
    public TMP_Text startCountdownText;

    [Header("Game Settings")]
    public float answerTimeLimit = 10f;
    public int baseScore = 10;
    public int bonusScore = 500;
    public int bonusLimitSeconds = 3;
    public int targetScore = 100;

    private float questionTimer;
    private bool isPlaying = false;
    private int score = 0;

    [Header("SFX")]
public AudioSource sfxSource;

public AudioClip countdownSFX;   // 🔢 เสียง 3 2 1
public AudioClip goSFX;          // 🚀 เสียง GO / เริ่ม
public AudioClip correctSFX;     // ✅ ตอบถูก
public AudioClip bonusSFX;       // ⭐ ได้โบนัส (ไวมาก)


    [Header("End FX")]
public Image fadeImage;
public TMP_Text bossResultText;
public float fadeDuration = 1f;

[Header("Audio")]
public AudioSource miniGameBGM;




    void Start()
    {

 if (sfxSource == null)
        sfxSource = GetComponent<AudioSource>();
         PauseManager pause = Object.FindFirstObjectByType<PauseManager>();

    if (pause != null)
        pause.isMiniGameActive = true; // บล็อก pause
        fastText.gameObject.SetActive(false);
    feedbackText.gameObject.SetActive(false);
    winPanel.SetActive(false);   // ✅ ปิด winPanel ก่อนเริ่ม
    StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
{
    startPanel.SetActive(true);
    int count = 10;

    while (count > 0)
    {
        startCountdownText.text = count.ToString();

        // 🔊 เสียงเคาท์ดาว
        if (sfxSource && countdownSFX)
            sfxSource.PlayOneShot(countdownSFX, 0.8f);

        yield return new WaitForSeconds(1f);
        count--;
    }

    // 🚀 GO
    startCountdownText.text = "GO!";

    if (sfxSource && goSFX)
        sfxSource.PlayOneShot(goSFX, 1f);

    yield return new WaitForSeconds(0.4f);

    // ❌ ซ่อนเท็กซ์เคาท์ดาว
    startPanel.SetActive(false);
    startCountdownText.text = "";

    StartQuiz();
}


   void StartQuiz()
{
    score = 0;
    UpdateScoreUI();
    currentIndex = 0;
    fastText.gameObject.SetActive(false);
    feedbackText.gameObject.SetActive(false);

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
                OnAnswerClicked(-1); // หมดเวลา = ตอบผิด
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
    isPlaying = true;  // ✅ เปิดให้เริ่มจับเวลาและตอบคำถามทันที
}


    void OnAnswerClicked(int index)
    {
        isPlaying = false;
        QuizQuestion q = questions[currentIndex];

       if (index == q.correctIndex)
{
    int gained = baseScore;

    // ✅ เสียงตอบถูก
    if (sfxSource && correctSFX)
        sfxSource.PlayOneShot(correctSFX, 0.7f);

    if (questionTimer >= answerTimeLimit - bonusLimitSeconds)
    {
        gained += bonusScore;

        // ⭐ เสียงโบนัส
        if (sfxSource && bonusSFX)
            sfxSource.PlayOneShot(bonusSFX, 0.9f);

        StartCoroutine(ShowFastText());
    }

    score += gained;
    UpdateScoreUI();

    if (score >= targetScore)
    {
        WinGame();
        return;
    }
}
        else
        {
            // ตอบผิด ลดหัวใจ + ข้อความ “ผิดแล้ว!”
            GameManager.Instance.TakeDamage(1);
            StartCoroutine(ShowFeedbackText("ผิดแล้ว!"));
        }

        currentIndex++;
        ShowQuestion();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator ShowFastText()
    {
        fastText.text = "ไวมาก!";
        fastText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        fastText.gameObject.SetActive(false);
    }

    IEnumerator ShowFeedbackText(string text)
    {
        feedbackText.text = text;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        feedbackText.gameObject.SetActive(false);
    }

    // void WinGame()
    // {
    //     isPlaying = false;
    //     timerText.text = "You Win!";
    //     winPanel.SetActive(true);
    // }

    void WinGame()
{
    isPlaying = false;
    StartCoroutine(WinSequence());
}

IEnumerator WinSequence()
{
    isPlaying = false;

    // 🔇 ปิดเพลงทั้งหมด
    if (miniGameBGM != null)
        miniGameBGM.Stop();

    if (AudioManager.Instance != null)
        AudioManager.Instance.StopMusic();

    // 🖤 เฟดดำ
    yield return StartCoroutine(FadeImage(0f, 1f));

    // 👑 ชื่อบอส
    yield return StartCoroutine(ShowBossText("Morrott"));

    // เว้นจังหวะนิดนึงให้หายใจ
    yield return new WaitForSeconds(1f);

    // ☠️ Defeated
    yield return StartCoroutine(ShowBossText("<color=#00B50C>Defeated</color>"));

    // เว้นอีกนิดก่อนกลับแมพ
    yield return new WaitForSeconds(2.5f);

    ReturnToMap();
}



IEnumerator FadeImage(float from, float to)
{
    float t = 0f;
    Color c = fadeImage.color;

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        c.a = Mathf.Lerp(from, to, t / fadeDuration);
        fadeImage.color = c;
        yield return null;
    }

    c.a = to;
    fadeImage.color = c;
}


IEnumerator ShowBossText(string text)
{
    bossResultText.text = text;
    bossResultText.alpha = 0f;

    // Fade In
    float t = 0f;
    float fadeInTime = 0.6f;

    while (t < fadeInTime)
    {
        t += Time.deltaTime;
        bossResultText.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
        yield return null;
    }

    bossResultText.alpha = 1f;

    // ⏸ ค้างข้อความ
    yield return new WaitForSeconds(1.4f);

    // Fade Out
    t = 0f;
    float fadeOutTime = 0.6f;

    while (t < fadeOutTime)
    {
        t += Time.deltaTime;
        bossResultText.alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
        yield return null;
    }

    bossResultText.alpha = 0f;
}




  public void ContinueToMap()
    {
        winPanel.SetActive(false);

        // ❌ ไม่ต้องย้าย Player, ใช้ตำแหน่งเดิม
        // Unfreeze Player + Map
        // UnfreezePlayerAndMap();
      Vector3 bossPos = FinalBoss.Instance.transform.position;

Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
if (player != null)
{
    Vector3 offset = new Vector3(8f, -3.5f, 0);
    player.position = bossPos + offset;
}

        // เรียกบอสตาย
        if (FinalBoss.Instance != null && FinalBoss.Instance.state != FinalBoss.BossState.Dead)
        {
            FinalBoss.Instance.BossDefeated();
        }
        // เก็บตำแหน่ง Player



        // Unload มินิเกม
        SceneManager.UnloadSceneAsync("MinigameFinal");

        if (GameManager.Instance != null)
    GameManager.Instance.isMiniGameActive = false;

        // ปิด MiniGame Active
        PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
        if (pause != null)
            pause.isMiniGameActive = false;

}

void ReturnToMap()
{
    // ย้าย player ใกล้บอส
    Vector3 bossPos = FinalBoss.Instance.transform.position;

    Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
    if (player != null)
    {
        Vector3 offset = new Vector3(8f, -3.5f, 0);
        player.position = bossPos + offset;
    }

    // เรียกบอสตาย
    if (FinalBoss.Instance != null &&
        FinalBoss.Instance.state != FinalBoss.BossState.Dead)
    {
        FinalBoss.Instance.BossDefeated();
    }

    // unload มินิเกม
    SceneManager.UnloadSceneAsync("MinigameFinal");

    // ปลดสถานะมินิเกม
    if (GameManager.Instance != null)
        GameManager.Instance.isMiniGameActive = false;

    PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
    if (pause != null)
        pause.isMiniGameActive = false;
}

}
