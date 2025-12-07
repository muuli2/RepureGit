using UnityEngine;
using TMPro;

public class InteractionPrompt04 : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptUI;     // UI ปุ่ม E/F/R (TextMeshPro)
    public TMP_Text keyText;        // Text สำหรับแสดงตัวอักษร
    public GameObject highlight;    // วงรัศมี/Highlight

    /// <summary>
    /// แสดง Prompt และวง Highlight
    /// </summary>
    /// <param name="key">ตัวอักษรที่โชว์ เช่น "E", "F", "R"</param>
    public void ShowPrompt(string key)
    {
        if (promptUI != null) promptUI.SetActive(true);
        if (keyText != null) keyText.text = key;
        if (highlight != null) highlight.SetActive(true);
    }

    /// <summary>
    /// ซ่อน Prompt และวง Highlight
    /// </summary>
    public void HidePrompt()
    {
        if (promptUI != null) promptUI.SetActive(false);
        if (highlight != null) highlight.SetActive(false);
    }
}
