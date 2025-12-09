using UnityEngine;
using TMPro;
using System.Collections;

public class IntroMinigame : MonoBehaviour
{
    public TMP_Text text;
    public float moveSpeed = 900f;
    public float blinkDuration = 1.5f;
    public float blinkSpeed = 6f;

    public IEnumerator ShowText(string message)
    {
        text.text = message;
        text.gameObject.SetActive(true);

        RectTransform rect = text.GetComponent<RectTransform>();

        // เริ่มจากซ้ายจอ
        rect.anchoredPosition = new Vector2(-Screen.width, 0);

        // เป้าหมาย (ตรงกลางจอ)
        Vector2 target = new Vector2(0, -2);

        // เลื่อนจากซ้าย → กลาง
        while (Vector2.Distance(rect.anchoredPosition, target) > 20f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                target,
                moveSpeed * Time.unscaledDeltaTime  // ✅ ใช้ unscaled
            );

            yield return null;
        }

        rect.anchoredPosition = target;

        // เอฟเฟ็กต์กระพริบ
        float timer = 0f;
        Color originalColor = text.color;

        while (timer < blinkDuration)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.unscaledTime * blinkSpeed)); // ✅ ใช้ unscaled
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            timer += Time.unscaledDeltaTime; // ✅ ใช้ unscaled
            yield return null;
        }

        text.color = originalColor;
        text.gameObject.SetActive(false);
    }
}
