using UnityEngine;
using UnityEngine.InputSystem;

public class TrashItem04 : MonoBehaviour
{
    public TrashType trashType;
    public float interactRange = 1.5f;
    private InteractionPrompt prompt;
    private Vector3 startPos;
    private Quaternion startRot;

    void Awake()
    {
        prompt = GetComponent<InteractionPrompt>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        PlayerBin04 playerBin = player.GetComponent<PlayerBin04>();
        if (!playerBin) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactRange)
        {
            // E ใช้ทั้งยกถังจากพื้นและถอนจากแท่น
            if (!playerBin.HasTrash04())
            {
                prompt?.ShowPrompt("E");
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    playerBin.PickUpTrash04(gameObject, trashType);
                    prompt?.HidePrompt();
                }
            }
        }
        else
        {
            prompt?.HidePrompt();
        }
    }

    public void ResetTrash()
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
