using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadePanel : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float holdTime = 1f;   // ⭐ เพิ่ม: ช่วงค้างตอนดำสนิท

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        // ⭐ ค้างตอนดำสนิท
        yield return new WaitForSeconds(holdTime);

        SceneManager.LoadScene(sceneName);
    }
}
