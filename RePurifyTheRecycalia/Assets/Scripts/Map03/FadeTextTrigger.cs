using UnityEngine;
using TMPro;
using System.Collections;

public class FadeTextTrigger : MonoBehaviour
{
    public TMP_Text fadeText;
    [TextArea] public string message;

    public float fadeInDuration = 1.5f;
    public bool playOnce = true;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (playOnce && triggered) return;

        triggered = true;
        StopAllCoroutines();
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        fadeText.text = message;
        fadeText.alpha = 0f;
        fadeText.gameObject.SetActive(true);

        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            fadeText.alpha = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            yield return null;
        }

        fadeText.alpha = 1f;
    }
}
