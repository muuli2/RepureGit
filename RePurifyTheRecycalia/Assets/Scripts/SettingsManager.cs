using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // Panel ของ Settings
    public Slider bgmSlider;
    public Slider sfxSlider;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    void Start()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);

        if(bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        if(sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    public void OpenSettings()
    {
        if(settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);
    }

    void OnBGMChanged(float value)
    {
        if(bgmSource != null) bgmSource.volume = value;
    }

    void OnSFXChanged(float value)
    {
        if(sfxSource != null) sfxSource.volume = value;
    }
}
