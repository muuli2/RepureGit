using UnityEngine;

public class TrashPuzzleManager : MonoBehaviour
{
    public static TrashPuzzleManager Instance;

    public PedestalSlot[] pedestals;
    public GameObject door;   // เปลี่ยนจาก DoorController → GameObject เลย

    void Awake()
    {
        Instance = this;
    }

    public void CheckPuzzle()
    {
        // เช็คว่าครบ 4 แท่นไหม
        foreach (var p in pedestals)
        {
            if (p.placedTrash == null)
                return;
        }

        // ถ้าวางครบ → ตรวจถูกต้อง
        foreach (var p in pedestals)
        {
            if (!p.IsCorrect())
                return;
        }

        // ถูกต้องทั้งหมด → ให้ประตูหายไป
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
