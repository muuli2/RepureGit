using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManage dialogueManager;   // ใช้ชื่อ DialogueManage
    [TextArea]
    public string[] sentences;

    private bool triggered = false;
    public bool upgradeGunAfterDialogue = false;
    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            pm.SetCanMove(false); // 🔒 หยุดผู้เล่นเดิน

            PlayerShoot ps = other.GetComponent<PlayerShoot>();
            if (ps != null)
                ps.canShoot = false; // 🔒 หยุดยิง

           dialogueManager.StartDialogue(sentences, pm, this);

            
        }
    }

    public void OnDialogueFinished(PlayerMovement player)
{
    if (upgradeGunAfterDialogue)
    {
        PlayerShoot ps = player.GetComponent<PlayerShoot>();
        if (ps != null)
        {
            ps.UpgradeGun();
        }
    }
}

public void AfterDialogueUpgrade(PlayerShoot ps)
{
    if (upgradeGunAfterDialogue && ps != null)
    ps.UpgradeGun();
    ToastMessage.Instance.Show("ได้รับการอัพเกรด!");
}


}
