using UnityEngine;

public class PlayerBin04 : MonoBehaviour
{
    public GameObject carriedTrash;
    public TrashType carriedType;
    [Header("Sound FX")]
public AudioSource sfxSource;
public AudioClip placeOnPedestalSFX;   // 🔊 เสียงวางบนแท่น


    public Transform holdPoint;   // จุดไว้บนหัว

    public bool HasTrash04() => carriedTrash != null;

    public void PickUpTrash04(GameObject trashObj, TrashType type)
    {
        if (HasTrash04()) return;

        carriedTrash = trashObj;
        carriedType = type;

        trashObj.transform.SetParent(holdPoint);
        trashObj.transform.localPosition = Vector3.zero;
        trashObj.transform.localRotation = Quaternion.identity;
    }

    public GameObject DropTrash()
    {
        if (!HasTrash04()) return null;

        GameObject obj = carriedTrash;
        carriedTrash.transform.SetParent(null);
        carriedTrash = null;

        return obj;
    }
}
