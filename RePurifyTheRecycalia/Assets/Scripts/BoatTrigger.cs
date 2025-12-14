using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections; // ✅ ต้องมีอันนี้

public class BoatTrigger : MonoBehaviour
{
    public GameObject confirmPanel;    
    public Button yesButton;
    public Button noButton;
    public TMP_Text warningText;

    private int requiredPoints = 0;

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
            warningText.gameObject.SetActive(false);  // 🔥 รีเซ็ตทุกครั้ง
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            confirmPanel.SetActive(false);
            warningText.gameObject.SetActive(false);
        }
    }

    public void OnYes()
{
    int currentScore = ScoreManage.Instance.totalScore;
    if(currentScore < requiredPoints)
    {
        warningText.text = "ค่าชำระล้างยังไม่ถึงนะ…";
        warningText.gameObject.SetActive(true);
        return;
    }

    ScoreManage.Instance.AddScore(-requiredPoints);
    confirmPanel.SetActive(false);

    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.LoadScene("Map05");
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name != "Map05") return;

    // เมื่อซีนโหลดเสร็จ
    GameManager.Instance.SpawnPlayer(GameManager.Instance.spawnPoint.position);
    MonsterManage.Instance?.ResetAllMonsters();
    // GameManager.Instance.ResetAllTrash();

    SceneManager.sceneLoaded -= OnSceneLoaded;
}



    public void OnNo()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);
    }
}
