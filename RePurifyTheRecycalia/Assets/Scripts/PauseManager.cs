// PauseManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject confirmPanel;

    [HideInInspector]
    public bool isMiniGameActive = false; // ✅ เช็คมินิเกม

    private enum ConfirmAction { None, Restart, Home }
    private ConfirmAction pendingAction = ConfirmAction.None;

    void Start()
    {
        pauseMenu.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isMiniGameActive) // ❌ บล็อก Pause ตอนมินิเกม
                TogglePauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
            ClosePauseMenu();
        else
            OpenPauseMenu();
    }

    public void ResumeGame() => ClosePauseMenu();

    public void RestartGame()
    {
        pendingAction = ConfirmAction.Restart;
        confirmPanel.SetActive(true);
    }

    public void GoHome()
    {
        pendingAction = ConfirmAction.Home;
        confirmPanel.SetActive(true);
    }

    public void ConfirmNo()
    {
        confirmPanel.SetActive(false);
        pendingAction = ConfirmAction.None;
    }

    public void ConfirmYes()
    {
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;

        if (pendingAction == ConfirmAction.Restart)
{
    GameManager.Instance.RestartFromPause();
}

        else if (pendingAction == ConfirmAction.Home)
        {
            SceneManager.LoadScene("MainMenu");
        }

        pendingAction = ConfirmAction.None;
    }
}
