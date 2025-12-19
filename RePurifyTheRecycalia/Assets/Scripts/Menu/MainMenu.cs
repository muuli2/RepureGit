using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsManager settingsManager;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip clickClip;

    void Start()
    {
        PauseManager pause = Object.FindFirstObjectByType<PauseManager>();
        if (pause != null)
            pause.isMiniGameActive = true; // บล็อก pause
    }

    void PlayClick()
    {
        if (sfxSource && clickClip)
            sfxSource.PlayOneShot(clickClip);
    }

    public void GoToScene(string sceneName)
    {
        PlayClick();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
        PlayClick();
        Application.Quit();
        Debug.Log("Application has quit.");
    }

    public void OpenSettings()
    {
        PlayClick();
        if (settingsManager != null)
            settingsManager.OpenSettings();
    }
}
