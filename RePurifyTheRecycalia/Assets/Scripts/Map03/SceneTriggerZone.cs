using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTriggerZone : MonoBehaviour
{
    public string sceneToLoad = "Map04"; // ชื่อซีนที่จะไป

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // โหลดซีนใหม่
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
