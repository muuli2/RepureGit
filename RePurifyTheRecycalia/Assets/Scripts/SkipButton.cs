using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkipButton : MonoBehaviour
{
    public CanvasGroup skipButtonGroup;   
    public float fadeTime = 1f;           
    public float delayBeforeShow = 2f;    
    public string nextSceneName = "CharacterSelect";

    private bool canSkip = false;

    void Start()
    {
        skipButtonGroup.alpha = 0f;
        skipButtonGroup.interactable = false;
        skipButtonGroup.blocksRaycasts = false;

        Invoke(nameof(ShowSkipButton), delayBeforeShow);
    }

    void ShowSkipButton()
    {
        StartCoroutine(FadeInButton());
    }

    IEnumerator FadeInButton()
    {
        float t = 0f;

        while (t < fadeTime)
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

        SceneManager.LoadScene(nextSceneName);
    }

    // เรียกเมื่อคัตซีนจบ  
    public void CutsceneFinished()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
