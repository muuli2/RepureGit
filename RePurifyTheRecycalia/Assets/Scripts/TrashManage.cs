using UnityEngine;

public class TrashManage : MonoBehaviour
{
    public static TrashManage Instance;

    // ลากขยะทั้งหมดใน Inspector (เหมือน allMonsters)
    public Trash[] allTrash;

    private void Awake()
    {
        Instance = this;
    }

    // ฟังก์ชันรีเซ็ตขยะทุกชิ้น
    public void ResetAllTrash()
    {
        foreach (var t in allTrash)
        {
            if (t != null)
                t.ResetTrash();
        }
    }
}
