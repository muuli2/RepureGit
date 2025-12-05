using UnityEngine;
using TMPro;

public class CheckPointText : MonoBehaviour
{
    public TMP_Text checkpointText;  // Text ที่จะโชว์
    public float showTime = 2f;      // เวลาที่แสดง (วินาที)

    private bool triggered = false;

    void Start()
    {
        if (checkpointText != null)
            checkpointText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;   // กันไม่ให้ซ้ำ
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(ShowCheckpoint());
    }

    private System.Collections.IEnumerator ShowCheckpoint()
    {
        if (checkpointText != null)
        {
            checkpointText.text = "Checkpoint!";
            checkpointText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(showTime);

        if (checkpointText != null)
            checkpointText.gameObject.SetActive(false);
    }
}
