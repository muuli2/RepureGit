using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGame01 : MonoBehaviour
{
    public static MiniGame01 Instance;

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

    public string mapSceneName = "Map01"; // ซีนแมพหลัก
    public TrashItem.TrashType targetTrashType = TrashItem.TrashType.Wet;

    private void Awake() { Instance = this; }

    private void Start()
{
    PauseManager pause = FindObjectOfType<PauseManager>();
    if (pause != null)
        pause.isMiniGameActive = true; // บล็อก pause

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

   public void UpdateHeartsUI()
    {
        int currentLives = GameManager.Instance.lives; // ใช้หัวใจจากแมพหลัก
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentLives ? heartFull : heartEmpty;
        }
    }

    public void CollectTrash(TrashItem trash)
    {
        int points = 100; // คะแนนคงที่, หรือใช้ trash.scoreValue ถ้ามี
        if (trash.trashType == targetTrashType)
        {
            AddScore(points);
        }
        else
        {
            // ลดเลือดใน GameManager โดยตรง
            GameManager.Instance.TakeDamage(1);
            UpdateHeartsUI();
        }

        Destroy(trash.gameObject);

        // เช็คหัวใจหมด -> GameOver มินิเกม
        if (GameManager.Instance.lives <= 0)
        {
            GameOver();
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

    // ❌ เอาออก ห้ามฆ่าบอสตรงนี้ เพราะบอสไม่อยู่ในซีน
    // if (Boss.Instance != null)
    //     Boss.Instance.BossDefeated();

    Time.timeScale = 0;
}



    // ปุ่ม Win Panel: ไปต่อ (กลับ Map01)
    // ปุ่ม Continue ใน WinPanel
public void ContinueToMap()
{
    Time.timeScale = 1;
    if (winPanel != null)
        winPanel.SetActive(false);
SceneManager.UnloadSceneAsync("MiniGame01").completed += (op) =>
{
    if (Boss.Instance != null && Boss.Instance.state != Boss.BossState.Dead)
    {
        Boss.Instance.BossDefeated();
    }

    PauseManager pause = FindObjectOfType<PauseManager>();
    if (pause != null)
        pause.isMiniGameActive = false;
};

}



    // ปุ่ม Win Panel: เล่นมินิเกมใหม่
    public void ReplayMinigame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MiniGame01", LoadSceneMode.Additive);
    }

    

    // ปุ่ม GameOver Panel: รีด่าน (กลับแมพ)
   // ปุ่ม GameOver Panel: รีด่าน (กลับแมพ)
public void RetryMap()
{
    Time.timeScale = 1;

    // ซ่อน GameOver UI
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    // unload minigame
    SceneManager.UnloadSceneAsync("MiniGame01").completed += (op) =>
    {

         PauseManager pause = FindObjectOfType<PauseManager>();
        if (pause != null)
        pause.isMiniGameActive = false;
        // ทำให้แมพหลักเป็น active scene
        Scene mainMapScene = SceneManager.GetSceneByName(mapSceneName);
        SceneManager.SetActiveScene(mainMapScene);

        // รีเซ็ต Player ไปยัง lastCheckpoint ของ GameManager
        GameObject player = GameManager.Instance.GetPlayer();
        if (player != null)
        {
            player.SetActive(true); 
            player.transform.position = GameManager.Instance.lastCheckpoint;

            // ✅ เปิด PlayerMovement ถ้ามันถูกปิด
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null)
                pm.enabled = true;

            // ✅ เปิด PlayerInput ถ้ามันถูกปิด
           
            // รีเซ็ตหัวใจเต็ม
            GameManager.Instance.lives = GameManager.Instance.maxLives;
            GameManager.Instance.UpdateHeartsUI();

            // ปิด panel เผื่อเปิดอยู่
            if (GameManager.Instance.gameOverPanel != null)
                GameManager.Instance.gameOverPanel.SetActive(false);
        }
        else
        {
            // ถ้า playerRef ยัง null → spawn ใหม่
            Vector3 spawnPos = GameManager.Instance.lastCheckpoint != Vector3.zero ?
                               GameManager.Instance.lastCheckpoint : GameManager.Instance.spawnPoint.position;
            GameManager.Instance.SpawnPlayer(spawnPos);
        }
    };
}



}
