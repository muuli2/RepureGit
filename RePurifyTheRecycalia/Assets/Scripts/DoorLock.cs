using UnityEngine;
using System.Collections;

public class DoorLock : MonoBehaviour
{
    public CanvasGroup messagePanel;     // กล่องข้อความ
    public float fadeDuration = 0.5f;

    public GameObject doorSprite;        // ประตู (ให้ปิด/เปิด)

    private Coroutine routine;

    private void Start()
    {
        messagePanel.alpha = 0;
        messagePanel.blocksRaycasts = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (MonsterManage.Instance.AllEnemiesCleared())
        {
            // เปิดประตู
            doorSprite.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            // แสดงข้อความ
            StartFade(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // ซ่อนข้อความเมื่อเดินออก
        StartFade(false);
    }

    void StartFade(bool fadeIn)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Fade(fadeIn));
    }

    IEnumerator Fade(bool fadeIn)
    {
        float start = messagePanel.alpha;
        float end = fadeIn ? 1 : 0;
        float t = 0;

        if (fadeIn)
            messagePanel.blocksRaycasts = true;

        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            messagePanel.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }

        messagePanel.alpha = end;

        if (!fadeIn)
            messagePanel.blocksRaycasts = false;
    }
}
