using UnityEngine;
using TMPro;

public class PlayerTrash : MonoBehaviour
{
    public Transform trashIconPoint;       // ตำแหน่งโชว์ขยะบนหัว
    public float comboTime = 10f;          // เวลา combo
    public TMP_Text comboText;             // แสดง Combo UI
    public TMP_Text feedbackText;          // แสดงข้อความชั่วคราว เช่น "ผิดประเภทแล้วล่ะ..."

    private GameObject currentTrashIcon;
    private TrashType currentTrashType;    // ชนิดขยะที่ถืออยู่
    private bool hasTrash = false;
    private float comboTimer = 0f;
    private int comboCount = 0;

    void Start()
    {
        if (comboText == null)
            comboText = GameObject.Find("ComboText").GetComponent<TMP_Text>();

        if (feedbackText == null)
            feedbackText = GameObject.Find("FeedbackText").GetComponent<TMP_Text>();

        feedbackText.text = "";
    }

    void Update()
    {
        // Combo Timer
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboCount = 0;
                comboText.text = "";
            }
        }

        // ขยะติดหัวผู้เล่นทุกเฟรม
        if (hasTrash && currentTrashIcon != null)
        {
            currentTrashIcon.transform.position = trashIconPoint.position;
        }

        // กด R → วางขยะลงพื้น
        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (hasTrash)
                DropTrashOnGround();
        }
    }

    // เก็บขยะ
    public void PickUpTrash(GameObject trash, TrashType trashType)
    {
        if (hasTrash) return;

        hasTrash = true;
        currentTrashIcon = trash;
        currentTrashType = trashType;

        // ปิด Rigidbody + Collider
        Rigidbody2D rb = currentTrashIcon.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        Collider2D col = currentTrashIcon.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        currentTrashIcon.transform.SetParent(trashIconPoint);
        currentTrashIcon.transform.localPosition = Vector3.zero;
        currentTrashIcon.transform.localRotation = Quaternion.identity;
    }

    // วางขยะลงถัง
    public void DropTrashIntoBin(TrashType binType)
    {
        if (!hasTrash) return;

        // ตรวจสอบชนิดขยะ
        if (currentTrashType != binType)
        {
            // แสดงข้อความผิดประเภท
            feedbackText.text = "<color=red>ผิดประเภทแล้วล่ะ...</color>";
            Invoke("ClearFeedback", 5f); // หายไปหลัง 2 วินาที

            // ลดหัวใจผู้เล่น
            GameManager.Instance.TakeDamage(1);

            // วางขยะลงพื้นแทน
            DropTrashOnGround();
            return;
        }

        hasTrash = false;
        Destroy(currentTrashIcon);

        // คะแนน + combo
        int points = 100;
        if (comboCount >= 4) points *= 2; // Combo 5+ = x2
        ScoreManage.Instance.AddScore(points);

        comboCount++;
        comboTimer = comboTime;
        comboText.text = comboCount >= 5 ? $"Combo x{comboCount}! (x2!)" : $"Combo x{comboCount}!";

        currentTrashIcon = null;
    }

    // วางขยะลงพื้น (ไม่ให้คะแนน)
    private void DropTrashOnGround()
    {
        if (!hasTrash) return;

        hasTrash = false;

        currentTrashIcon.transform.SetParent(null);

        Rigidbody2D rb = currentTrashIcon.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        Collider2D col = currentTrashIcon.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        currentTrashIcon = null;
    }

    public bool HasTrash()
    {
        return hasTrash;
    }

    private void ClearFeedback()
    {
        feedbackText.text = "";
    }
}
