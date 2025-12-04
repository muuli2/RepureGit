using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public float interactRange = 1.5f;
    public TrashType binType;   // กำหนดชนิดถังใน Inspector
    private InteractionPrompt prompt;

    void Start()
    {
        prompt = GetComponent<InteractionPrompt>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerTrash playerTrash = player.GetComponent<PlayerTrash>();
        if (playerTrash == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= interactRange)
        {
            prompt?.ShowPrompt("F");

            if (UnityEngine.InputSystem.Keyboard.current.fKey.wasPressedThisFrame && playerTrash.HasTrash())
            {
                playerTrash.DropTrashIntoBin(binType);
            }
        }
        else
        {
            prompt?.HidePrompt();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
