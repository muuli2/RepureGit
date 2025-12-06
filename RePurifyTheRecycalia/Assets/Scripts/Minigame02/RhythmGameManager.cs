using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem; // <--- สำคัญ

public class RhythmMiniGame : MonoBehaviour
{
    public static RhythmMiniGame Instance;

    [Header("Gameplay")]
    public GameObject[] trashPrefabs;      // Prefab ขยะทุกชนิด
    public Transform[] spawnPositions;     // 4 lane: D F J K
    public float spawnInterval = 0.8f;     // ความถี่สปอนขยะ

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

    private bool gameStarted = false;
    private float timer = 0f;

    private void Awake() { Instance = this; }

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

   void Update()
{
    if (!gameStarted) return;

    timer += Time.deltaTime;
    if (timer >= spawnInterval)
    {
        SpawnTrash();
        timer = 0f;
    }

   var keyboard = Keyboard.current;

if (keyboard.dKey.wasPressedThisFrame) HitTrash(Key.D);
if (keyboard.fKey.wasPressedThisFrame) HitTrash(Key.F);
if (keyboard.jKey.wasPressedThisFrame) HitTrash(Key.J);
if (keyboard.kKey.wasPressedThisFrame) HitTrash(Key.K);
}
    void SpawnTrash()
{
    int numberOfLanes = Random.Range(1, 3); // 1 หรือ 2 เลน
    int[] lanes = new int[] {0, 1, 2, 3};

    // สุ่มลำดับเลน
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

        // สุ่ม prefab ขยะ
        int prefabIndex = Random.Range(0, trashPrefabs.Length);
        GameObject trash = Instantiate(trashPrefabs[prefabIndex], spawnPositions[lane].position, Quaternion.identity);
        TrashNote note = trash.GetComponent<TrashNote>();

        // ตั้งปุ่มตามเลน
        switch (lane)
        {
            case 0: note.correctKey = Key.D; break;
            case 1: note.correctKey = Key.F; break;
            case 2: note.correctKey = Key.J; break;
            case 3: note.correctKey = Key.K; break;
        }

        // ตั้งชนิดขยะ (ถ้ามี)
        note.trashType = Random.value > 0.5f ? TrashNote.TrashType.General : TrashNote.TrashType.Wet;
    }
}


 void HitTrash(Key key)
{
    TrashNote[] notes = FindObjectsOfType<TrashNote>();

    foreach (var note in notes)
    {
        if (Mathf.Abs(note.transform.position.y - note.hitY) <= note.hitRange &&
            note.correctKey == key)
        {
            // ส่งชนิดขยะเป้าหมายของด่านด้วย
            note.TryHit(key, targetTrashType);
        }
    }
}




KeyCode ConvertKey(Key key)
{
    switch (key)
    {
        case Key.D: return KeyCode.D;
        case Key.F: return KeyCode.F;
        case Key.J: return KeyCode.J;
        case Key.K: return KeyCode.K;
        default: return KeyCode.None;
    }
}

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score >= targetScore) WinGame();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
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

        if (GameManager.Instance.lives <= 0) GameOver();
    }

    void GameOver()
    {
        gameStarted = false;
        gameOverPanel.SetActive(true);

        TrashNote[] notes = FindObjectsOfType<TrashNote>();
        foreach (var t in notes) Destroy(t);
    }

    void WinGame()
    {
        gameStarted = false;
        winPanel.SetActive(true);

        TrashNote[] notes = FindObjectsOfType<TrashNote>();
        foreach (var t in notes) Destroy(t);
    }

    public void ContinueToMap()
    {
        winPanel.SetActive(false);
        SceneManager.UnloadSceneAsync("RhythmMiniGame");
    }

    public void RetryMap()
    {
        SceneManager.LoadScene("Map03");
    }

    

}
