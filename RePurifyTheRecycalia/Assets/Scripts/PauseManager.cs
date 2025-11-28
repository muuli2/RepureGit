using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;     // assign ใน Inspector
    public GameObject confirmPanel;  // assign ใน Inspector

    private enum ConfirmAction { None, Restart, Home }
    private ConfirmAction pendingAction = ConfirmAction.None;

    void Start()
    {
        // เริ่มเกมให้ซ่อน Pause และ Confirm
        pauseMenu.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        bool isPaused = pauseMenu.activeSelf;
        pauseMenu.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f; // หยุดหรือเล่นเกม
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

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

    public void ConfirmYes()
    {
        confirmPanel.SetActive(false);

        if (pendingAction == ConfirmAction.Restart)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (pendingAction == ConfirmAction.Home)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu"); // เปลี่ยนชื่อเป็นซีน Home จริง
        }

        pendingAction = ConfirmAction.None;
    }

    public void ConfirmNo()
    {
        confirmPanel.SetActive(false);
        pendingAction = ConfirmAction.None;
    }
}
