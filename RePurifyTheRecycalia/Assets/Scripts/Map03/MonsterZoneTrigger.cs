using UnityEngine;

public class MonsterZoneTrigger : MonoBehaviour
{
    [Header("Monster Spawn Settings")]
    public GameObject[] monsterPrefabs;  // Prefab มอนสเตอร์
    public int minSpawn = 10;            // spawn ขั้นต่ำ
    public int maxSpawn = 20;            // spawn สูงสุด
    public Vector2 spawnAreaMin;         // พื้นที่ spawn min (x, y)
    public Vector2 spawnAreaMax;         // พื้นที่ spawn max (x, y)

    [Header("Player Checkpoint")]
    public Transform map03Checkpoint;    // ตำแหน่ง checkpoint Map03

    private bool triggered = false;      // กัน spawn ซ้ำ

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered) return;

        if (col.CompareTag("Player"))
        {
            triggered = true;

            SpawnMonsters();

            // ถ้าอยากให้ player กลับ Map03 เมื่อตาย ให้บันทึก checkpoint
            GameManager.Instance.lastCheckpoint = map03Checkpoint.position;
        }
    }

    void SpawnMonsters()
    {
        int spawnCount = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            // เลือก prefab มอนสเตอร์สุ่ม
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            // สุ่มตำแหน่งในพื้นที่
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 spawnPos = new Vector3(x, y, 0);

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}
