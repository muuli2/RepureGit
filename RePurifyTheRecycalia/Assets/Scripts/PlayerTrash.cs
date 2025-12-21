using UnityEngine;
using TMPro;

public class PlayerTrash : MonoBehaviour
{
    [Header("Icon")]
    public Transform trashIconPoint;       // จุดโชว์ไอคอนบนหัว
    public GameObject trashIconPrefab;     // prefab ไอคอนขยะ

    [Header("Combo")]
    public float comboTime = 10f;
    public TMP_Text comboText;
    public TMP_Text feedbackText;

    [Header("Sound FX")]
    public AudioSource sfxSource;
    public AudioClip correctTrashSFX;
    public AudioClip wrongTrashSFX;

    // ===== Runtime =====
    private GameObject currentTrashIcon;    // ไอคอนบนหัว
    private GameObject currentTrashObject;  // ขยะจริงในฉาก
    private TrashType currentTrashType;

    private bool hasTrash = false;
    private float comboTimer = 0f;
    private int comboCount = 0;

    void Start()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        if (comboText == null)
            comboText = GameObject.Find("ComboText")?.GetComponent<TMP_Text>();

        if (feedbackText == null)
            feedbackText = GameObject.Find("FeedbackText")?.GetComponent<TMP_Text>();

        if (feedbackText != null)
            feedbackText.text = "";
    }

    void Update()
    {
        // ===== Combo Timer =====
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboCount = 0;
                if (comboText != null)
                    comboText.text = "";
            }
        }

        // กด R → วางขยะลงพื้น
        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (hasTrash)
                DropTrashOnGround();
        }
    }

    // ================== PICK UP ==================
    public void PickUpTrash(GameObject trash, TrashType trashType)
    {
        if (hasTrash) return;

        hasTrash = true;
        currentTrashType = trashType;

        // 🔹 เก็บขยะจริงไว้ แต่ไม่เอาขึ้นหัว
        currentTrashObject = trash;
        currentTrashObject.SetActive(false);

        // 🔹 สร้างไอคอนบนหัว
        currentTrashIcon = Instantiate(trashIconPrefab, trashIconPoint);
        currentTrashIcon.transform.localPosition = Vector3.zero;
        currentTrashIcon.transform.localRotation = Quaternion.identity;

        // คัดลอก sprite จากขยะจริง
SpriteRenderer trashSR = currentTrashObject.GetComponent<SpriteRenderer>();
SpriteRenderer iconSR = currentTrashIcon.GetComponent<SpriteRenderer>();

if (trashSR != null && iconSR != null)
{
    iconSR.sprite = trashSR.sprite;
    iconSR.flipX = trashSR.flipX;
    iconSR.flipY = trashSR.flipY;
}

    }

    // ================== DROP INTO BIN ==================
    public void DropTrashIntoBin(TrashType binType)
    {
        if (!hasTrash) return;

        // ❌ ผิดถัง
        if (currentTrashType != binType)
        {
            if (sfxSource && wrongTrashSFX)
                sfxSource.PlayOneShot(wrongTrashSFX);

            if (feedbackText != null)
            {
                feedbackText.text = "<color=red>ผิดประเภทแล้วล่ะ...</color>";
                Invoke(nameof(ClearFeedback), 2f);
            }

            GameManager.Instance?.TakeDamage(1);
            DropTrashOnGround();
            return;
        }

        // ✅ ถูกถัง
        if (sfxSource && correctTrashSFX)
            sfxSource.PlayOneShot(correctTrashSFX);

        // ลบ icon
        if (currentTrashIcon != null)
            Destroy(currentTrashIcon);

        // ลบขยะจริง (หรือ SetActive(false) ถ้าจะ reuse)
        if (currentTrashObject != null)
            Destroy(currentTrashObject);

        hasTrash = false;
        currentTrashIcon = null;
        currentTrashObject = null;

        // ===== Score + Combo =====
        int points = 100;
        if (comboCount >= 4) points *= 2;

        ScoreManage.Instance?.AddScore(points);

        comboCount++;
        comboTimer = comboTime;

        if (comboText != null)
        {
            comboText.text = comboCount >= 5
                ? $"Combo x{comboCount}! (x2!)"
                : $"Combo x{comboCount}!";
        }

        TrashCheck.Instance?.AddCollected();
    }

    // ================== DROP ON GROUND ==================
    private void DropTrashOnGround()
    {
        if (!hasTrash) return;

        hasTrash = false;

        // ลบไอคอน
        if (currentTrashIcon != null)
            Destroy(currentTrashIcon);

        // เอาขยะจริงกลับโลก
        if (currentTrashObject != null)
        {
            currentTrashObject.SetActive(true);
            currentTrashObject.transform.position =
                transform.position + Vector3.right; // ปรับตำแหน่งได้
        }

        currentTrashIcon = null;
        currentTrashObject = null;
    }

    public bool HasTrash()
    {
        return hasTrash;
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }
}
