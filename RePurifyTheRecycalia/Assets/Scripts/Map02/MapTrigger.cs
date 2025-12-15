using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject confirmPanel;
    public Button yesButton;
    public Button noButton;
    public TMP_Text warningText;

    [Header("Map Settings")]
    public string targetSceneName;     // ← ซีนปลายทาง
    public int requiredPoints = 0;     // ← คะแนนที่ต้องใช้

     private PlayerShoot playerShoot;

    private void Start()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYes);
        noButton.onClick.AddListener(OnNo);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            confirmPanel.SetActive(true);
            warningText.gameObject.SetActive(false);

              playerShoot = col.GetComponent<PlayerShoot>();
        if (playerShoot != null)
            playerShoot.canShoot = false; // ❌ ห้ามยิง
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            confirmPanel.SetActive(false);
            warningText.gameObject.SetActive(false);

            
        if (playerShoot != null)
            playerShoot.canShoot = true; // ✅ ยิงได้อีก
        }
    }

    public void OnYes()
    {

         if (playerShoot != null)
        playerShoot.canShoot = false;
        int currentScore = ScoreManage.Instance.totalScore;

        if (currentScore < requiredPoints)
        {
            warningText.text = "ค่าชำระล้างยังไม่ถึงนะ…";
            warningText.gameObject.SetActive(true);
            return;
        }

        // หักแต้ม
        ScoreManage.Instance.AddScore(-requiredPoints);

        confirmPanel.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != targetSceneName) return;

        // กลับมาจุด Spawn ปกติของ GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SpawnPlayer(
                GameManager.Instance.spawnPoint.position
            );
        }

        MonsterManage.Instance?.ResetAllMonsters();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnNo()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);
    }
}
