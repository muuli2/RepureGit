using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGameStart : MonoBehaviour
{
    public GameObject rulesPanel;      // Panel กติกา
    public TMP_Text countdownText;     // Text สำหรับนับถอยหลัง
    public GameObject gameManager;     // MiniGame01 หรือ Object ที่ควบคุมเกม

    public float countdownTime = 3f;   // 3 2 1

    void Start()
    {
        rulesPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        gameManager.SetActive(false);  // ปิดเกมไว้ก่อน
    }

    public void OnStartButton()
    {
        rulesPanel.SetActive(false);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
{
    int count = 3;
    while(count > 0)
    {
        countdownText.text = count.ToString();
        yield return new WaitForSeconds(1f);
        count--;
    }

    countdownText.text = "GO!";
    yield return new WaitForSeconds(1f);

    countdownText.gameObject.SetActive(false);
    gameManager.SetActive(true);  // เปิด GameManager
    MiniGame01.Instance.gameStarted = true; // เริ่ม Spawn ขยะ
}

}
