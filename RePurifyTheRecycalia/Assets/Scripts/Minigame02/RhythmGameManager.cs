using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class RhythmMiniGame : MonoBehaviour
{
    public static RhythmMiniGame Instance;

    [Header("Gameplay")]
    public GameObject[] trashPrefabs;
    public Transform[] spawnPositions;
    public float spawnInterval = 0.8f;

    [Header("Score / Lives")]
    public int targetScore = 2500;
    private int score = 0;

    [Header("Current Stage")]
    public TrashNote.TrashType targetTrashType = TrashNote.TrashType.General;

    [Header("UI")]
    public TMP_Text scoreText;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject rulesPanel;
    public TMP_Text countdownText;

    [Header("Bins FX")]
    public GameObject binD;
    public GameObject binF;
    public GameObject binJ;
    public GameObject binK;

    // เก็บ coroutine แยกกันสำหรับ D F J K
    private Coroutine flashD;
    private Coroutine flashF;
    private Coroutine flashJ;
    private Coroutine flashK;

    public float highlightTime = 0.15f;
    public Color highlightColor = Color.white;

    private bool gameStarted = false;
    private float timer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        countdownText.gameObject.SetActive(false);
        rulesPanel.SetActive(true);

        score = 0;
        UpdateScoreUI();
        UpdateHeartsUI();
    }

    public void StartGameFromRules()
    {
        rulesPanel.SetActive(false);
        StartCoroutine(CountdownAndStart());
    }

    IEnumerator CountdownAndStart()
    {
        countdownText.gameObject.SetActive(true);
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);

        gameStarted = true;
    }

    private void Update()
    {
        if (!gameStarted) return;

        // Spawn note
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTrash();
            timer = 0f;
        }

        var keyboard = Keyboard.current;

        // --- กดปุ่ม + เล่นเอฟเฟกต์แสงถังแบบไม่ค้าง ---
        if (keyboard.dKey.wasPressedThisFrame)
        {
            HitTrash(Key.D);
            StartHighlight(ref flashD, binD);
        }

        if (keyboard.fKey.wasPressedThisFrame)
        {
            HitTrash(Key.F);
            StartHighlight(ref flashF, binF);
        }

        if (keyboard.jKey.wasPressedThisFrame)
        {
            HitTrash(Key.J);
            StartHighlight(ref flashJ, binJ);
        }

        if (keyboard.kKey.wasPressedThisFrame)
        {
            HitTrash(Key.K);
            StartHighlight(ref flashK, binK);
        }
    }

    void SpawnTrash()
    {
        int numberOfLanes = Random.Range(1, 3); // 1–2 notes ต่อรอบ
        int[] lanes = new int[] { 0, 1, 2, 3 };

        // shuffle lanes
        for (int i = 0; i < lanes.Length; i++)
        {
            int j = Random.Range(i, lanes.Length);
            int temp = lanes[i];
            lanes[i] = lanes[j];
            lanes[j] = temp;
        }

        for (int i = 0; i < numberOfLanes; i++)
        {
            int lane = lanes[i];
            int prefabIndex = Random.Range(0, trashPrefabs.Length);

            GameObject trash = Instantiate(trashPrefabs[prefabIndex], spawnPositions[lane].position, Quaternion.identity);
            TrashNote note = trash.GetComponent<TrashNote>();

            switch (lane)
            {
                case 0: note.correctKey = Key.D; break;
                case 1: note.correctKey = Key.F; break;
                case 2: note.correctKey = Key.J; break;
                case 3: note.correctKey = Key.K; break;
            }
        }
    }

    // ============================
    //   กดถังแล้วสว่าง (ไม่ค้าง)
    // ============================
    void StartHighlight(ref Coroutine routine, GameObject bin)
    {
        if (bin == null) return;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FlashBin(bin));
    }

    IEnumerator FlashBin(GameObject bin)
    {
        var img = bin.GetComponent<Image>();
        var sr = bin.GetComponent<SpriteRenderer>();

        Color original;

        if (img != null)
        {
            original = img.color;
            img.color = highlightColor;
            yield return new WaitForSeconds(highlightTime);
            img.color = original;
        }
        else if (sr != null)
        {
            original = sr.color;
            sr.color = highlightColor;
            yield return new WaitForSeconds(highlightTime);
            sr.color = original;
        }
    }

    // ============================
    //       จับจังหวะกดถูก
    // ============================
    void HitTrash(Key key)
    {
        TrashNote[] notes = FindObjectsOfType<TrashNote>();

        foreach (var note in notes)
        {
            if (Mathf.Abs(note.transform.position.y - note.hitY) <= note.hitRange &&
                note.correctKey == key)
            {
                note.TryHit(key, targetTrashType);
            }
        }
    }

    // ============================
    //    Score / Health / UI
    // ============================
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score >= targetScore)
            WinGame();
    }

    void UpdateScoreUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
    }

    public void UpdateHeartsUI()
    {
        int lives = GameManager.Instance.lives;

        for (int i = 0; i < heartImages.Length; i++)
            heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
    }

    public void LoseLife()
    {
        GameManager.Instance.TakeDamage(1);
        UpdateHeartsUI();

        if (GameManager.Instance.lives <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameStarted = false;
        gameOverPanel.SetActive(true);

        foreach (var t in FindObjectsOfType<TrashNote>())
            Destroy(t.gameObject);
    }

    void WinGame()
    {
        gameStarted = false;
        winPanel.SetActive(true);

        foreach (var t in FindObjectsOfType<TrashNote>())
            Destroy(t.gameObject);
    }

    // ============================
    //       Navigation
    // ============================
    public void ContinueToMap()
    {
        winPanel.SetActive(false);
        SceneManager.UnloadSceneAsync("Minigame02");
    }

    public void RetryMap()
    {
        SceneManager.LoadScene("Map03");
    }
}
