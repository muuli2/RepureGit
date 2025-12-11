using UnityEngine;

public class AreaNameTrigger : MonoBehaviour
{
    public string areaName;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        AreaTextController.Instance.ShowAreaName(areaName);
    }
}
