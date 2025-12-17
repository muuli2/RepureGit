using UnityEngine;

public class MusicTriggerZone : MonoBehaviour
{
    public AudioClip musicToPlay;
    public float fadeTime = 2f;

    private bool hasPlayed = false;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = true;

        AudioManager.Instance.PlayMusicFade(musicToPlay, fadeTime);

        // ❌ ปิด trigger ทิ้ง ไม่ให้ยิงซ้ำเด็ดขาด
        if (col != null)
            col.enabled = false;
    }
}
