using UnityEngine;
using TMPro;
using System.Collections;

public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance;
    public TMP_Text toastText;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip toastClip;

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

        // 🔊 เล่นเสียงตอน Toast ขึ้น
        if (audioSource && toastClip)
            audioSource.PlayOneShot(toastClip);

        toastText.CrossFadeAlpha(1f, 0.4f, false);  // เฟดเข้า
        
        yield return new WaitForSeconds(2f);       // ค้างไว้

        toastText.CrossFadeAlpha(0f, 0.5f, false); // เฟดออก
    }
}
