using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public float cooldown = 10f;
    private float cooldownTimer = 0f;
    public Animator anim;
    public bool unlocked = false;       // ยังไม่ปลดล็อกตอนเริ่ม
    public GameObject skillSprite;      

    [Header("Skill UI")]
    public Image skillIcon;
    public Sprite skillReadySprite;
    public Sprite skillCooldownSprite;

    [Header("Skill Attack")]
    public GameObject skillAreaPrefab;
    public float skillDamage = 10f;

    void Start()
{
    if (skillSprite != null)
        skillSprite.SetActive(unlocked); // ถ้ายังไม่ปลดล็อกให้ซ่อน
}


    void Update()
    {
        // ✔ ถ้ายังไม่ได้ปลดล็อกสกิล (ยังไม่คุยกับอาจารย์)
        if (!GameManager.Instance.skillUnlocked) 
            return;

        // ✔ ลดคูลดาวน์
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            
            if (cooldownTimer <= 0f)
                skillIcon.sprite = skillReadySprite; // พร้อมใช้
        }

        // ✔ กด Shift เพื่อใช้สกิล
        if (UnityEngine.InputSystem.Keyboard.current.leftShiftKey.wasPressedThisFrame ||
            UnityEngine.InputSystem.Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            TryUseSkill();
        }
    }

    void TryUseSkill()
    {
        if (cooldownTimer > 0f) 
            return;

        // ⭐ เล่นอนิเมชันท่าสกิล
        anim.SetTrigger("Skill");

        // ⭐ สร้างวงสกิลทำดาเมจรอบตัว
        Instantiate(skillAreaPrefab, transform.position, Quaternion.identity);

        // ⭐ ตั้งคูลดาวน์
        cooldownTimer = cooldown;
        skillIcon.sprite = skillCooldownSprite;
    }

     public void UnlockSkill()
    {
        unlocked = true;
        if (skillSprite != null)
            skillSprite.SetActive(true);
    }
}
