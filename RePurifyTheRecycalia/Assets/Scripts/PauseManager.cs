using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject confirmPanel;

    [HideInInspector]
    public bool isMiniGameActive = false;

    private enum ConfirmAction { None, Restart, Home }
    private ConfirmAction pendingAction = ConfirmAction.None;

    void Start()
    {
        pauseMenu.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isMiniGameActive)
                TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
            ClosePauseMenu();
        else
            OpenPauseMenu();
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
    // ปิด UI
    if (confirmPanel != null) confirmPanel.SetActive(false);
    if (pauseMenu != null) pauseMenu.SetActive(false);
    Time.timeScale = 1f;

    if (pendingAction == ConfirmAction.Restart)
    {
        GameManager.Instance?.RestartGame();
    }
    else if (pendingAction == ConfirmAction.Home)
    {

        if (GameManager.Instance != null)
{
    Destroy(GameManager.Instance.gameObject);
}
SceneManager.LoadScene("MainMenu");

    }

    pendingAction = ConfirmAction.None;
}


public void ResetPauseMenu()
{
    pauseMenu.SetActive(false);
    confirmPanel.SetActive(false);
    Time.timeScale = 1f;
    pendingAction = ConfirmAction.None;
}



}
