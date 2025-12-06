using UnityEngine;

public class TrashSpawner01 : MonoBehaviour
{
    [Header("Trash Prefabs (หลายชนิด)")]
    public GameObject[] trashPrefabs;

    [Header("ตำแหน่งสปอน (จุดวางขยะ)")]
    public Transform[] spawnPoints;

    [Header("จำนวนขยะที่จะเกิด")]
    public int spawnCount = 10;

    public void ResetAllTrash()
    {
        // ลบขยะเก่าออกทั้งหมด
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash2");
        foreach (var t in trashObjects)
            Destroy(t);

        // เกิดใหม่ random 10 ชิ้น
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(prefab, point.position, Quaternion.identity);
        }
    }

    // ใช้ตอนเข้าแมพครั้งแรก
    public void SpawnInitialTrash()
    {
        ResetAllTrash();
    }
}
