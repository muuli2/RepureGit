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
    private PlayerShoot playerShoot;

    [Header("Sound FX")]
public AudioSource sfxSource;
public AudioClip successYesSFX;   // 🔔 เสียงผ่านเงื่อนไข



    private int requiredPoints = 0;

    private void Start()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);

         if (sfxSource == null)
        sfxSource = GetComponent<AudioSource>();

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
    if(currentScore < requiredPoints)
    {
        warningText.text = "ค่าชำระล้างยังไม่ถึงนะ…";
        warningText.gameObject.SetActive(true);
        return;
    }

     if (sfxSource && successYesSFX)
        sfxSource.PlayOneShot(successYesSFX, 0.8f);

    ScoreManage.Instance.AddScore(-requiredPoints);
    confirmPanel.SetActive(false);

    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.LoadScene("Map02");
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name != "Map02") return;

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
