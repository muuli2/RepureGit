using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 2.0f;
    private bool isFading = false;


    void Awake()
    {
        // ให้ fadeCanvas เริ่มใส
        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

   public IEnumerator FadeInBlack()
{
    if (isFading) yield break; // ❌ ถ้ากำลังเฟดอยู่ ให้หยุดเลย
    isFading = true; // 🔒 ล็อกเฟด

    float t = 0;
    fadeCanvas.gameObject.SetActive(true);

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
        yield return null;
    }

    fadeCanvas.alpha = 1f;

    // 🔓 จะปลดล็อกไหม?
    // ถ้าเฟดดำถาวรจนไปฉากใหม่ ไม่ต้องปลดล็อก
    // isFading = false;
}



}
