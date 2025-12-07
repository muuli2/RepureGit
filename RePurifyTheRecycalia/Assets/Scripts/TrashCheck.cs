using UnityEngine;

public class TrashCheck : MonoBehaviour
{
    public static TrashCheck Instance;

    public int totalTrash;
    public int collectedTrash = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddCollected()
    {
        collectedTrash++;
        if (collectedTrash > totalTrash)
            collectedTrash = totalTrash;
    }

    public bool AllTrashCollected()
    {
        return collectedTrash >= totalTrash;
    }

    public void ResetTrash()
    {
        collectedTrash = 0;
    }
}
