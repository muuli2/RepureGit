using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsManager settingsManager;
     private PlayerShoot playerShoot;

    void start ()
    {
        PauseManager pause = Object.FindFirstObjectByType<PauseManager>();

    if (pause != null)
        pause.isMiniGameActive = true; // บล็อก pause
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }

    public void OpenSettings()
    {
        if(settingsManager != null)
            settingsManager.OpenSettings();
    }
}
