using UnityEngine;

public class SceneSpawnPoint : MonoBehaviour
{
     void Start()
    {
        var gm = GameManager.Instance;
        if (gm != null)
        {
            // หา spawn point ของ map03
            var spawn = GameObject.FindGameObjectWithTag("RespawnPoint2");
            if (spawn != null)
            {
                gm.SpawnPlayer(spawn.transform.position);
            }
        }
    }
}
