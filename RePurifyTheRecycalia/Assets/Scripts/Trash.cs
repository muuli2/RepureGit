using UnityEngine;

public class Trash : MonoBehaviour
{
    public TrashType trashType;
    public float interactRange = 1.5f;

    private InteractionPrompt prompt;
    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake()
    {
        prompt = GetComponent<InteractionPrompt>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void ResetTrash()
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
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
}
