using UnityEngine;

public class Map03Spawner : MonoBehaviour
{
    void Start()
    {
        var gm = GameManager.Instance;
        if (gm != null)
        {
            // หา spawn point ของ map03
            var spawn = GameObject.FindGameObjectWithTag("RespawnPoint");
            if (spawn != null)
            {
                gm.SpawnPlayer(spawn.transform.position);
            }
        }
    }
}
