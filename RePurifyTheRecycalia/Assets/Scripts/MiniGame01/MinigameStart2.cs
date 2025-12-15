using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGameStart2 : MonoBehaviour
{
    public GameObject rulesPanel;      // Panel กติกา
    public TMP_Text countdownText;     // Text สำหรับนับถอยหลัง
    public GameObject gameManager;     // MiniGame01 หรือ Object ที่ควบคุมเกม

    public float ruleShowTime = 5f;    // แสดงกติกา 6 วินาที
    public float countdownTime = 3f;   // 3 2 1

    [Header("Sound")]
public AudioSource audioSource;
public AudioClip countdownBeep; // เสียง 3 2 1
public AudioClip goSound;       // เสียง GO!


    void Start()
    {
        rulesPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        gameManager.SetActive(false);  // ปิดเกมไว้ก่อน

        StartCoroutine(AutoStartFlow());
    }

   IEnumerator AutoStartFlow()
{
    // 1) แสดงกติกา
    yield return new WaitForSeconds(ruleShowTime);

    // 2) เริ่มนับถอยหลัง
    rulesPanel.SetActive(false);
    countdownText.gameObject.SetActive(true);

    int count = 3;
    while (count > 0)
    {
        countdownText.text = count.ToString();

        // 🔊 เสียงติ๊บ
        if (audioSource != null && countdownBeep != null)
            audioSource.PlayOneShot(countdownBeep);

        yield return new WaitForSeconds(1f);
        count--;
    }

    // 3) GO!
    countdownText.text = "GO!";

    // 🔊 เสียง GO
    if (audioSource != null && goSound != null)
        audioSource.PlayOneShot(goSound);

    yield return new WaitForSeconds(1f);

    countdownText.gameObject.SetActive(false);
    gameManager.SetActive(true);

    // 4) เริ่มเกม
    MiniGame012.Instance.gameStarted = true;
}


    
}
