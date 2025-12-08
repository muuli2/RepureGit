using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGameStart2 : MonoBehaviour
{
    public GameObject rulesPanel;      // Panel กติกา
    public TMP_Text countdownText;     // Text สำหรับนับถอยหลัง
    public GameObject gameManager;     // MiniGame01 หรือ Object ที่ควบคุมเกม

    public float ruleShowTime = 4f;    // แสดงกติกา 6 วินาที
    public float countdownTime = 3f;   // 3 2 1

    void Start()
    {
        rulesPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        gameManager.SetActive(false);  // ปิดเกมไว้ก่อน

        StartCoroutine(AutoStartFlow());
    }

    IEnumerator AutoStartFlow()
    {
        // 1) แสดงกติกา 6 วินาที
        yield return new WaitForSeconds(ruleShowTime);

        // 2) ปิดกติกา และเริ่มนับถอยหลัง
        rulesPanel.SetActive(false);
        countdownText.gameObject.SetActive(true);

        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        // 3) GO!
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        gameManager.SetActive(true);

        // 4) สั่งเริ่มเกมจริง
        MiniGame012.Instance.gameStarted = true;
    }
}
