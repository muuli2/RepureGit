using UnityEngine;
using UnityEngine.InputSystem;

public class Map01SceneManager : MonoBehaviour
{
    public GameObject KnightPrefab;
    public GameObject MagePrefab;
    public Transform spawnPoint;

    void Start()
    {
        GameObject toSpawn = null;

        if (SelectedCharacter.characterName == "Knight")
            toSpawn = KnightPrefab;
        else if (SelectedCharacter.characterName == "Mage")
            toSpawn = MagePrefab;

        if (toSpawn != null)
        {
            GameObject player = Instantiate(toSpawn, spawnPoint.position, Quaternion.identity);

            // ตรวจสอบ PlayerInput
            if (player.GetComponent<PlayerInput>() == null)
            {
                player.AddComponent<PlayerInput>();
            }

            // ตัวละครพร้อมเดินทันที, Rigidbody2D assign ใน PlayerController Awake
        }
        else
        {
            Debug.LogWarning("Prefab ไม่ถูกตั้งค่า หรือ characterName ผิด!");
        }
    }
}
