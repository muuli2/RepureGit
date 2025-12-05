// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;

// public class Checkpoint : MonoBehaviour
// {
//     public Transform checkpointPoint;
//     public TMP_Text checkpointText;
//     public float displayTime = 2f;
//     public bool isFirstCheckpoint = false;
//     public float captureRadius = 10f;
//      public int checkpointIndex; // ★ ลำดับเช็กพอยท์

//     private bool hasActivated = false;

//     [HideInInspector] public List<GameObject> monsters = new();
//     [HideInInspector] public List<GameObject> trashItems = new();
//     [HideInInspector] public List<GameObject> items = new();

//     private void Start()
//     {
//         if (checkpointText != null)
//             checkpointText.gameObject.SetActive(false);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && !hasActivated)
//         {
//             hasActivated = true;
//             GameManager.Instance.lastCheckpoint = checkpointPoint.position;
//             GameManager.Instance.currentCheckpoint = this;

//             if (checkpointText != null)
//                 StartCoroutine(ShowCheckpointText());

//             CaptureObjectsInZone();

//             ScoreManage.Instance?.SaveScoreAtCheckpoint();

//             if (isFirstCheckpoint)
//             {
//                 ResetZone();
//             }
//         }
//     }

//     private IEnumerator ShowCheckpointText()
//     {
//         checkpointText.gameObject.SetActive(true);
//         checkpointText.text = "Checkpoint!";
//         yield return new WaitForSeconds(displayTime);
//         checkpointText.gameObject.SetActive(false);
//     }

//     private void CaptureObjectsInZone()
//     {
//         monsters.Clear();
//         trashItems.Clear();
//         items.Clear();

//         Collider2D[] colliders = Physics2D.OverlapCircleAll(checkpointPoint.position, captureRadius);
//         foreach (var col in colliders)
//         {
//             if (col.CompareTag("Monster")) monsters.Add(col.gameObject);
//             if (col.CompareTag("Trash")) trashItems.Add(col.gameObject);
//             if (col.CompareTag("Item")) items.Add(col.gameObject);
//         }
//     }

//     public void ResetZone()
//     {
//         foreach (var monster in monsters)
//         {
//             if (monster != null)
//             {
//                 monster.SetActive(true);
//                 var m = monster.GetComponent<Monster>();
//                 if (m != null) m.ResetHealth();
//             }
//         }

//         foreach (var trash in trashItems)
//             if (trash != null) trash.SetActive(true);

//         foreach (var item in items)
//             if (item != null) item.SetActive(true);

//         ScoreManage.Instance?.ResetScoreAfterCheckpoint();
//     }

//     public void ResetCheckpoint()
//     {
//         hasActivated = false;
//     }

//     // ดูวงกลมใน Scene View
//     private void OnDrawGizmosSelected()
//     {
//         if (checkpointPoint == null) return;

//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(checkpointPoint.position, captureRadius);
//     }
// }
