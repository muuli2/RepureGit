using UnityEngine;
using TMPro;
using System.Collections;

public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance;
    public TMP_Text toastText;

    void Awake()
    {
        Instance = this;
        toastText.canvasRenderer.SetAlpha(0f);
    }

    public void Show(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ToastRoutine(message));
    }

    IEnumerator ToastRoutine(string message)
    {
        toastText.text = message;
        toastText.CrossFadeAlpha(1f, 0.4f, false);  // เฟดเข้า 0.4 วิ
        
        yield return new WaitForSeconds(2f);         // ค้างไว้ 2 วิ

        toastText.CrossFadeAlpha(0f, 0.5f, false);  // เฟดออก 0.5 วิ
    }
}
