using UnityEngine;

public class TrashPuzzleManager : MonoBehaviour
{
    public static TrashPuzzleManager Instance;

    public PedestalSlot[] pedestals;
    public GameObject door;   // เปลี่ยนจาก DoorController → GameObject เลย

    [Header("SFX")]
public AudioSource audioSource;
public AudioClip correctClip;

private bool puzzleSolved = false; // กันเสียงดังซ้ำ


    void Awake()
    {
        Instance = this;
    }

  public void CheckPuzzle()
{
    if (puzzleSolved) return;

    // เช็คว่าครบ 4 แท่นไหม
    foreach (var p in pedestals)
    {
        if (p.placedTrash == null)
            return;
    }

    // ตรวจความถูกต้อง
    foreach (var p in pedestals)
    {
        if (!p.IsCorrect())
            return;
    }

    // ✅ ถูกต้องทั้งหมด
    puzzleSolved = true;

    // 🔊 เล่นเสียงถูกต้อง
    if (audioSource && correctClip)
        audioSource.PlayOneShot(correctClip);

    // เปิดทาง / ประตูหาย
    if (door != null)
        door.SetActive(false);
}

}
