using UnityEngine;

public class MonsterManage : MonoBehaviour
{
    public static MonsterManage Instance;

    public int totalEnemies;     
    public int killedEnemies = 0;

    // ลากมอนสเตอร์ทั้งหมดใน Inspector
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

    // ✅ ฟังก์ชันรีเซ็ตมอน
   public void ResetAllMonsters()
{
    killedEnemies = 0;

    foreach (var m in allMonsters)
    {
        if (m != null)
            m.ResetMonster(); // จะทำให้มอนทั้งหมดกลับตำแหน่งเดิม + health เต็ม + active
    }
}


}
