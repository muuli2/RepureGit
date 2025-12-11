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
}
