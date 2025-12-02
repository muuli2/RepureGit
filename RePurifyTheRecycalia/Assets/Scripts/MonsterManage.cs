using UnityEngine;

public class MonsterManage : MonoBehaviour
{
    public static MonsterManage Instance;

    public int totalEnemies;     // จำนวนมอนทั้งหมดในแมพ
    public int killedEnemies = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void EnemyKilled()
    {
        killedEnemies++;

        // กันจำนวนล้น
        if (killedEnemies > totalEnemies)
            killedEnemies = totalEnemies;
    }

    public bool AllEnemiesCleared()
    {
        return killedEnemies >= totalEnemies;
    }
}
