using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoCutscene : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup skipButtonGroup;
    public float fadeTime = 1f;
    public float delayBeforeShow = 2f;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public string nextSceneName = "CharacterSelect";

    private bool canSkip = false;

    void Start()
    {
        // ปุ่ม Skip เริ่มโปร่งใสและกดไม่ได้
        if(skipButtonGroup != null)
        {
            skipButtonGroup.alpha = 0f;
            skipButtonGroup.interactable = false;
            skipButtonGroup.blocksRaycasts = false;
        }

        // ปุ่มโผล่ช้า
        Invoke(nameof(ShowSkipButton), delayBeforeShow);

        // ตรวจสอบจบคลิป
        if(videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;

        // เล่นวิดีโอทันที
        videoPlayer.Play();
    }

    void ShowSkipButton()
    {
        StartCoroutine(FadeInSkip());
    }

    IEnumerator FadeInSkip()
    {
        float t = 0f;
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            skipButtonGroup.alpha = t / fadeTime;
            yield return null;
        }

        skipButtonGroup.interactable = true;
        skipButtonGroup.blocksRaycasts = true;
        canSkip = true;
    }

    public void OnSkipPressed()
    {
        if (!canSkip) return;
        LoadNextScene();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
