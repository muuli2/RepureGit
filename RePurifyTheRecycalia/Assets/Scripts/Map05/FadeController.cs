using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1.0f;

    void Awake()
    {
        // ให้ fadeCanvas เริ่มใส
        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

    public IEnumerator FadeInBlack()
{
    Debug.Log("FadeInBlack ถูกเรียกแล้ว!");

    float t = 0;
    fadeCanvas.gameObject.SetActive(true);

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
        Debug.Log("Alpha = " + fadeCanvas.alpha);
        yield return null;
    }

    fadeCanvas.alpha = 1f;
    Debug.Log("Fade เสร็จ!");
}

}
