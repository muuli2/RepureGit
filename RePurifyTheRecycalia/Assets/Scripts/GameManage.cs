using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
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
                player.AddComponent<PlayerInput>();

            // Assign Rigidbody2D ให้ PlayerMovement
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null && movement.rb == null)
                movement.rb = player.GetComponent<Rigidbody2D>();

            // ให้ Main Camera ตามตัวละคร
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
                camFollow.target = player.transform;
        }
        else
        {
            Debug.LogWarning("Prefab ไม่ถูกตั้งค่า หรือ characterName ผิด!");
        }
    }
}
