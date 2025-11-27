using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;   // Trash_Wet, Trash_Dry, Trash_Recycle
    public float spawnInterval = 4f;
    public float spawnRangeX = 8f;
    public float spawnY = 6f;

    private float timer = 0f;

    void Update()
    {
        if (!MiniGame01.Instance.gameStarted) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTrash();
            timer = 0f;
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Length == 0) return;

        int index = Random.Range(0, trashPrefabs.Length);
        GameObject prefab = trashPrefabs[index];

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
