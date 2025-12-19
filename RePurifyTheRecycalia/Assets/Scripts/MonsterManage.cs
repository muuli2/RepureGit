using UnityEngine;

public class MonsterManage : MonoBehaviour
{
    public static MonsterManage Instance;

    public int totalEnemies;
    public int killedEnemies = 0;
    public Monster[] allMonsters;

    private void Awake()
    {
        Instance = this;
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        if (killedEnemies > totalEnemies)
            killedEnemies = totalEnemies;
    }

    public bool AllEnemiesCleared()
    {
        return killedEnemies >= totalEnemies;
    }

    // 🔹 รีเฉพาะ "ตัวมอน" (ห้ามยุ่งกับ killedEnemies)
    public void ResetAllMonsters()
    {
        foreach (var m in allMonsters)
        {
            if (m != null)
                m.ResetMonster();
        }
    }

    // 🔹 รีเฉพาะตัวนับ (ใช้ตอนเริ่มแมพใหม่เท่านั้น)
    public void ResetCounter()
    {
        killedEnemies = 0;
    }
}
