using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour
{
    public CanvasGroup popup;
    public float fadeInDuration = 1f;    // เวลาเฟดเข้า
    public float showTime = 15f;         // เวลาที่อยู่บนจอ
    public float fadeOutDuration = 1f;   // เวลาเฟดออก

    void Start()
    {
        StartCoroutine(ShowPopup());
    }

    IEnumerator ShowPopup()
    {
        popup.alpha = 0;
        popup.blocksRaycasts = true;

        // ---- Fade In ----
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeInDuration;
            popup.alpha = t;
            yield return null;
        }
        popup.alpha = 1;

        // ---- Wait ----
        yield return new WaitForSeconds(showTime);

        // ---- Fade Out ----
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeOutDuration;
            popup.alpha = 1 - t;
            yield return null;
        }

        popup.alpha = 0;
        popup.blocksRaycasts = false;
    }
}
