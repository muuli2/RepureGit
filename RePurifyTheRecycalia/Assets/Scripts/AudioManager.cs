using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    // public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetBGMVolume(float v) => bgmSource.volume = v;
    // public void SetSFXVolume(float v) => sfxSource.volume = v;

    public void PlayMusic(AudioClip newMusic)
{
    if (bgmSource.clip == newMusic)
        return; // ถ้าเพลงเดียวกันไม่ต้องเล่นซ้ำ

    bgmSource.Stop();     // หยุดเพลงเก่า
    bgmSource.clip = newMusic;
    bgmSource.Play();    // เล่นเพลงใหม่
}

 public void StopMusic()
    {
        bgmSource.Stop();
    }

}
