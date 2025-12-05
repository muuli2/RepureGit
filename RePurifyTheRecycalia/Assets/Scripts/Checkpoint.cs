// Checkpoint.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Transform checkpointPoint;
    public TMP_Text checkpointText;
    public float displayTime = 2f;

    private bool hasActivated = false;

    private void Start()
    {
        if (checkpointText != null)
            checkpointText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            hasActivated = true;
            GameManager.Instance.lastCheckpoint = checkpointPoint.position;

            if (checkpointText != null)
                StartCoroutine(ShowCheckpointText());
        }
    }

    private IEnumerator ShowCheckpointText()
    {
        checkpointText.gameObject.SetActive(true);
        checkpointText.text = "Checkpoint!";
        yield return new WaitForSeconds(displayTime);
        checkpointText.gameObject.SetActive(false);
    }

    public void ResetCheckpoint()
    {
        hasActivated = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (checkpointPoint != null)
            Gizmos.DrawWireSphere(checkpointPoint.position, 1f);
    }
}
