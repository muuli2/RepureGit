using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Prefabs")]
    public GameObject KnightPrefab;
    public GameObject MagePrefab;
    public Transform spawnPoint;

    [Header("Lives System")]
    public int maxLives = 5;
    public int lives;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button restartButton;

    [HideInInspector]
    public Vector3 lastCheckpoint;

    [HideInInspector]
    // public Checkpoint currentCheckpoint; // checkpoint ปัจจุบัน

    private GameObject playerRef;
    public bool isMiniGameActive = false;


    public GameObject GetPlayer()
{
    return playerRef;
}
    public int mapPoints = 0; // คะแนนสะสมในแมพ



   private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        lives = maxLives;          // ✅ รีหัวใจเต็ม
        mapPoints = 0;             // ✅ รีคะแนน
    }
    else
    {
        Destroy(gameObject);
    }
}


    private void Start()
    {

        UpdateHeartsUI();
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartFromGameOver);

        RestartFromScene();
    }

    public void RestartFromPause()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene("Map01");
}

public void RestartFromGameOver()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene("Map01");
}


    private void RestartFromScene()
    {
        // รีเซ็ตหัวใจเต็ม
        lives = maxLives;
        UpdateHeartsUI();

        // ปิด panel ทุกอัน
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Vector3 spawnPos = lastCheckpoint != Vector3.zero ? lastCheckpoint : spawnPoint.position;
        SpawnPlayer(spawnPos);

        // รีเซ็ต checkpoint ทุกตัว
    //     foreach (var cp in Object.FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))

    // cp.ResetCheckpoint();
    }

    public void SpawnPlayer(Vector3 position)
{
    if (playerRef != null)
    {
        playerRef.SetActive(true);
        playerRef.transform.position = position;

        // ✅ เปิดสคริปต์ PlayerMovement และ Rigidbody
        var movement = playerRef.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        var rb = playerRef.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        return;
    }

    GameObject toSpawn = null;
    if (SelectedCharacter.characterName == "Knight")
        toSpawn = KnightPrefab;
    else if (SelectedCharacter.characterName == "Lumina")
        toSpawn = MagePrefab;

    if (toSpawn != null)
    {
        playerRef = Instantiate(toSpawn, position, Quaternion.identity);

        if (playerRef.GetComponent<PlayerInput>() == null)
            playerRef.AddComponent<PlayerInput>();

        var movement = playerRef.GetComponent<PlayerMovement>();
        if (movement != null && movement.rb == null)
            movement.rb = playerRef.GetComponent<Rigidbody2D>();

        var camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.target = playerRef.transform;
    }
    else
    {
        Debug.LogWarning("Prefab ไม่ถูกตั้งค่า หรือ characterName ผิด!");
    }
}


    public void TakeDamage(int amount)
    {
        lives -= amount;
        lives = Mathf.Clamp(lives, 0, maxLives);
        UpdateHeartsUI();

        if (lives <= 0)
            PlayerDied();
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
            heartImages[i].sprite = i < lives ? heartFull : heartEmpty;
    }

    void PlayerDied()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (playerRef != null)
            playerRef.SetActive(false);
    }

//     public void ResetToCurrentCheckpoint()
// {
//     if (currentCheckpoint == null)
//     {
//         // ถ้ายังไม่มี checkpoint → รีสตาร์ททั้งแมพ
//         RestartFromScene();
//         return;
//     }

//     // รีเซ็ตตำแหน่ง player
//     playerRef.transform.position = currentCheckpoint.checkpointPoint.position;
//     playerRef.SetActive(true);

//     // รีหัวใจ
//     lives = maxLives;
//     UpdateHeartsUI();

//     // รีสิ่งของ/มอนสเตอร์ใน zone หลัง checkpoint ปัจจุบัน
//     currentCheckpoint.ResetZone();
// }

//  public void RestartFromPauseOrGameOver()
//     {
//         if (currentCheckpoint == null || currentCheckpoint.isFirstCheckpoint)
//         {
//             // รีสตาร์ททั้งแมพ
//             RestartFromScene();
//         }
//         else
//         {
//             // รีเฉพาะ zone หลัง checkpoint ปัจจุบัน
//             ResetToCurrentCheckpoint();
//         }
//     }


// private Checkpoint FindCurrentCheckpoint()
// {
//     Checkpoint nearestCP = null;
//     float minDist = float.MaxValue;
//     foreach (var cp in Object.FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
//     {
//         float dist = Vector3.Distance(playerRef.transform.position, cp.checkpointPoint.position);
//         if (dist < minDist)
//         {
//             minDist = dist;
//             nearestCP = cp;
//         }
//     }
//     return nearestCP;
// }






    
}




