using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BoatTrigger : MonoBehaviour
{
    public GameObject confirmPanel;    
    public Button yesButton;
    public Button noButton;
    public TMP_Text warningText;

    private int requiredPoints = 2500;

    private void Start()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYes);
        noButton.onClick.AddListener(OnNo);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            confirmPanel.SetActive(true);
            warningText.gameObject.SetActive(false);  // 🔥 รีเซ็ตทุกครั้ง
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            confirmPanel.SetActive(false);
            warningText.gameObject.SetActive(false);
        }
    }

    private void OnYes()
    {
        // 🔥 สำคัญ: รีเซ็ต warning ก่อนเช็กแต้ม
        warningText.gameObject.SetActive(false);

        int currentScore = ScoreManage.Instance.totalScore;

        if (currentScore < requiredPoints)
        {
            warningText.text = "ค่าชำระล้างยังไม่ถึงนะ…";
            warningText.gameObject.SetActive(true);
            return;
        }

        // หักแต้ม 2500
        ScoreManage.Instance.AddScore(-requiredPoints);

        // 🔥 ปิด panel ทันที กัน UI กระพริบ
        confirmPanel.SetActive(false);

        // โหลดซีนถัดไป
        SceneManager.LoadScene("Map02");
    }

    private void OnNo()
    {
        confirmPanel.SetActive(false);
        warningText.gameObject.SetActive(false);
    }
}
