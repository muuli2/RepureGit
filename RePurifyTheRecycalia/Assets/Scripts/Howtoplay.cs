using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour
{
    public CanvasGroup popup;
    public float fadeInDuration = 1f;
    public float showTime = 15f;
    public float fadeOutDuration = 1f;

    private bool hasShown = false;   // กันไม่ให้โชว์ซ้ำ

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasShown) return;

        if (other.CompareTag("Player"))
        {
            hasShown = true;
            StartCoroutine(ShowPopup());
        }
    }

    IEnumerator ShowPopup()
    {
        popup.alpha = 0;
        popup.blocksRaycasts = true;

        // Fade In
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeInDuration;
            popup.alpha = t;
            yield return null;
        }

        popup.alpha = 1;
        yield return new WaitForSeconds(showTime);

        // Fade Out
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
