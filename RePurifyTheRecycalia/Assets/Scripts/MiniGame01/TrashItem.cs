using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public enum TrashType { Wet, Dry, Recycle, Hazard }  // ประเภทขยะ
    public TrashType trashType;
}
