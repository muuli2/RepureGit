using UnityEngine;
using System.Collections;

public class SignInfo : MonoBehaviour
{
    public CanvasGroup infoPanel;      // หน้าข้อมูล
    public float fadeDuration = 0.5f;  // เวลาเฟด

    private Coroutine currentRoutine;

    private void Start()
    {
        infoPanel.alpha = 0;               // ซ่อนก่อน
        infoPanel.blocksRaycasts = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(true);  // fade in
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(false); // fade out
        }
    }

    void StartFade(bool fadeIn)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadePanel(fadeIn));
    }

    IEnumerator FadePanel(bool fadeIn)
    {
        float start = infoPanel.alpha;
        float end = fadeIn ? 1 : 0;
        float t = 0;

        if (fadeIn)
            infoPanel.blocksRaycasts = true;

        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            infoPanel.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }

        infoPanel.alpha = end;

        if (!fadeIn)
            infoPanel.blocksRaycasts = false;
    }
}
