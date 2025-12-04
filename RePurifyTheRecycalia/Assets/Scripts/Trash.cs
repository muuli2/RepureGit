using UnityEngine;

public class Trash : MonoBehaviour
{
    public TrashType trashType;          // ชนิดขยะ
    public float interactRange = 1.5f;   // ระยะเก็บขยะ
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

        if (distance <= interactRange && !playerTrash.HasTrash())
        {
            prompt?.ShowPrompt("E");

            if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                playerTrash.PickUpTrash(gameObject, trashType);
            }
        }
        else
        {
            prompt?.HidePrompt();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
