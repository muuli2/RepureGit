using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public TMP_Text promptText;     // TextMeshPro สำหรับโชว์ข้อความ
    public Transform target;        // จุดที่ให้ข้อความปรากฏ (บนหัวขยะ/ผู้เล่น)
    public Vector3 offset = new Vector3(0, 1f, 0); // ขยับข้อความขึ้นเหนือวัตถุ

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false); // ซ่อนตอนเริ่ม
    }

    void Update()
    {
        if (promptText != null && target != null)
        {
            // ให้ข้อความตามตำแหน่ง target + offset
            promptText.transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }

    // เรียกเมื่อผู้เล่นเข้าใกล้
    public void ShowPrompt(string message)
    {
        if (promptText != null)
        {
            promptText.text = message;
            promptText.gameObject.SetActive(true);
        }
    }

    // ซ่อนข้อความ
    public void HidePrompt()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
