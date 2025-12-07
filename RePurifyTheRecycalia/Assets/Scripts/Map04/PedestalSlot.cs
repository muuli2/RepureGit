using UnityEngine;
using UnityEngine.InputSystem;

public class PedestalSlot : MonoBehaviour
{
    public TrashType correctType;
    public GameObject placedTrash;   // ถังบนแท่น
    public Transform placePoint;     // จุดวางบนแท่น
    public float placementRadius = 2f;

    private InteractionPrompt prompt;

    void Awake()
    {
        prompt = GetComponent<InteractionPrompt>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        PlayerBin04 playerTrash = player.GetComponent<PlayerBin04>();
        if (!playerTrash) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= placementRadius)
        {
            if (placedTrash != null)
            {
                // มีถังบนแท่น → ใช้ E ถอนถัง
                prompt?.ShowPrompt("E");
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    playerTrash.PickUpTrash04(placedTrash, placedTrash.GetComponent<TrashItem04>().trashType);
                    placedTrash = null;
                    TrashPuzzleManager.Instance.CheckPuzzle();
                }
            }
            else if (playerTrash.HasTrash04())
            {
                // ไม่มีถังบนแท่น + ถือถัง → F วางแท่น / R วางพื้น
                prompt?.ShowPrompt("F / R");

                if (Keyboard.current.fKey.wasPressedThisFrame)
                {
                    placedTrash = playerTrash.DropTrash();
                    placedTrash.transform.SetParent(placePoint);
                    placedTrash.transform.localPosition = Vector3.zero;
                    TrashPuzzleManager.Instance.CheckPuzzle();
                }

                if (Keyboard.current.rKey.wasPressedThisFrame)
                {
                    GameObject droppedTrash = playerTrash.DropTrash();
                    droppedTrash.transform.SetParent(null);
                    TrashPuzzleManager.Instance.CheckPuzzle();
                }
            }
        }
        else
        {
            // อยู่นอกรัศมี → ซ่อน Prompt
            prompt?.HidePrompt();

            // R วางพื้นจาก anywhere
            if (playerTrash.HasTrash04() && Keyboard.current.rKey.wasPressedThisFrame)
            {
                GameObject droppedTrash = playerTrash.DropTrash();
                droppedTrash.transform.SetParent(null);
            }
        }
    }

    public bool IsCorrect()
    {
        if (placedTrash == null) return false;
        return placedTrash.GetComponent<TrashItem04>().trashType == correctType;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, placementRadius);
    }
}
