using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

      private Coroutine fadeRoutine;

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

 public void PlayMusicFade(AudioClip newMusic, float fadeTime = 1.5f)
    {
        if (bgmSource.clip == newMusic && bgmSource.isPlaying)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeInMusic(newMusic, fadeTime));
    }

    IEnumerator FadeInMusic(AudioClip newMusic, float fadeTime)
    {
        bgmSource.Stop();
        bgmSource.clip = newMusic;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 1f, t / fadeTime);
            yield return null;
        }

        bgmSource.volume = 1f;
    }

 public void StopMusic()
    {
        bgmSource.Stop();
    }

}
