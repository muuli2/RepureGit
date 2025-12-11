using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;   // ← ต้องมี

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject confirmPanel;
    
public Slider bgmSlider;
// public Slider sfxSlider;

    [HideInInspector]
    public bool isMiniGameActive = false;
      // 🔥 รายชื่อซีนที่ห้าม Pause
    private readonly string[] noPauseScenes = 
    {
        "MainMenu",
        "CharacterSelect",
        "CutScenes",
        "End"
    };


    private enum ConfirmAction { None, Restart, Home }
    private ConfirmAction pendingAction = ConfirmAction.None;

   

    void Start()
{
    pauseMenu.SetActive(false);
    confirmPanel.SetActive(false);

    if (AudioManager.Instance != null)
    {
        bgmSlider.value = AudioManager.Instance.bgmSource.volume;
        // sfxSlider.value = AudioManager.Instance.sfxSource.volume;

        bgmSlider.onValueChanged.AddListener((v) => AudioManager.Instance.SetBGMVolume(v));
        // sfxSlider.onValueChanged.AddListener((v) => AudioManager.Instance.SetSFXVolume(v));
    }
}


 void Update()
    {
        string scene = SceneManager.GetActiveScene().name;

        // ❌ ซีนเหล่านี้กด ESC แล้วไม่ต้อง Pause
        if (noPauseScenes.Contains(scene))
            return;

        // ✔ ซีนอื่น Pause ได้
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

//         if (GameManager.Instance != null)
// {
//     Destroy(GameManager.Instance.gameObject);
// }
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
