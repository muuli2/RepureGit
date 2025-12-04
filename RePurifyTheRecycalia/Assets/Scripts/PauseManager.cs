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
        pauseMenu.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        // กด ESC เพื่อสลับ Pause Menu
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePauseMenu();
        }
    }

    // --- ฟังก์ชันสำหรับเรียกจากปุ่ม UI ---
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // หยุดเกม
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // เล่นเกมต่อ
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
            ClosePauseMenu();
        else
            OpenPauseMenu();
    }

    // --- ฟังก์ชันปุ่ม Resume, Restart, Home ---
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
            SceneManager.LoadScene("MainMenu"); // เปลี่ยนเป็นชื่อ Scene จริง
        }

        pendingAction = ConfirmAction.None;
    }

    public void ConfirmNo()
    {
        confirmPanel.SetActive(false);
        pendingAction = ConfirmAction.None;
    }
}
