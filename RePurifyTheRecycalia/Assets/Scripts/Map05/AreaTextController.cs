using UnityEngine;
using TMPro;
using System.Collections;

public class AreaTextController : MonoBehaviour
{
    public static AreaTextController Instance;

    public TMP_Text areaText;
    public float moveSpeed = 1500f;
    public float blinkSpeed = 1.5f;
    public float blinkDuration = 3f; // กระพริบ 3 วิ
    public float fadeOutTime = 1f;   // จางหาย 1 วิ

    private RectTransform rect;

    void Awake()
    {
        Instance = this;
        rect = GetComponent<RectTransform>();
        areaText.alpha = 0f;
    }

    public void ShowAreaName(string text)
    {
        StopAllCoroutines();
        StartCoroutine(ShowTextRoutine(text));
    }

    IEnumerator ShowTextRoutine(string text)
    {
        areaText.text = text;
        areaText.alpha = 1f;

        // เริ่มจากนอกซ้ายจอ
        rect.anchoredPosition = new Vector2(-Screen.width, 0);

        Vector2 targetPos = Vector2.zero;  // กลางจอ

        // -------- 1) เลื่อนเข้ามาตรงกลาง --------
        while (Vector2.Distance(rect.anchoredPosition, targetPos) > 1f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                targetPos,
                moveSpeed * Time.unscaledDeltaTime
            );
            yield return null;
        }

        // -------- 2) กระพริบ 3 วิ --------
        float timer = 0f;
        while (timer < blinkDuration)
        {
            areaText.alpha = 1f;
            yield return new WaitForSecondsRealtime(blinkSpeed);

            areaText.alpha = 0f;
            yield return new WaitForSecondsRealtime(blinkSpeed);

            timer += blinkSpeed * 2f;
        }

        // -------- 3) ค่อยๆเฟดหาย --------
        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.unscaledDeltaTime;
            areaText.alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            yield return null;
        }

        areaText.alpha = 0f;
    }
}
