using UnityEngine;
using TMPro;
using System.Collections;

public class IntroMinigame : MonoBehaviour
{
    public TMP_Text text;
    public float moveSpeed = 900f;
    public float blinkDuration = 1.5f;  // เวลากระพริบ
    public float blinkSpeed = 6f;       // ความถี่กระพริบ

    public IEnumerator ShowText(string message)
    {
        text.text = message;
        text.gameObject.SetActive(true);

        RectTransform rect = text.GetComponent<RectTransform>();

        // เริ่มจากซ้ายจอ
        rect.anchoredPosition = new Vector2(-Screen.width, 0);

        // เป้าหมาย (ตรงกลางจอ)
        Vector2 target = new Vector2(0, 5);

        // เลื่อนจากซ้าย → กลาง
        while (Vector2.Distance(rect.anchoredPosition, target) > 20f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // ตั้งให้พอดีกลางชัวร์ ๆ
        rect.anchoredPosition = target;

        // เอฟเฟ็กต์กระพริบ
        float timer = 0f;
        Color originalColor = text.color;

        while (timer < blinkDuration)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        // คืนสี
        text.color = originalColor;

        // ซ่อนข้อความ
        text.gameObject.SetActive(false);
    }
}
