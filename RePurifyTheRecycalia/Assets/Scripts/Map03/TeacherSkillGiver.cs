using UnityEngine;

public class TeacherSkillGiver : MonoBehaviour
{
    public GameObject dialogPanel;   // UI กล่องบทสนทนา
    public SkillManager playerSkill; // อ้างถึง Player.SkillManager

    private bool given = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !given)
        {
            dialogPanel.SetActive(true);
        }
    }

    // เรียกจากปุ่มใน UI “จบการสนทนา”
   public void FinishDialog()
{
    dialogPanel.SetActive(false);

    if (!given)
    {
        // ปลดล็อกสกิล
        playerSkill.unlocked = true;

        // แสดงสกิลบนตัวผู้เล่น
        if (playerSkill.skillSprite != null)
            playerSkill.skillSprite.SetActive(true);

        given = true;
    }
}


}
