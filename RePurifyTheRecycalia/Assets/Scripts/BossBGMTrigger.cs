using UnityEngine;

public class BossBGMTrigger : MonoBehaviour
{
    public BossMap01 boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        boss?.StartBossBGM();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        boss?.StopBossBGM();
    }
}
