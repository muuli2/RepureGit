using UnityEngine;

public class MusicTriggerZone : MonoBehaviour
{
    public AudioClip musicToPlay;  // เพลงที่อยากให้เล่นเมื่อเดินเข้า
    public bool playOnce = false;  // กันไม่ให้เล่นซ้ำๆ ถ้าต้องการ
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (playOnce && hasPlayed) return;

      AudioManager.Instance.PlayMusicFade(musicToPlay, 2f); // 🌊 เฟดขึ้น
        hasPlayed = true;
    }
}
