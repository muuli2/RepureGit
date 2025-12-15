using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 1f; // กี่วินาทีลดเลือดครั้ง

    private Coroutine damageRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (damageRoutine != null)
                StopCoroutine(damageRoutine);
        }
    }

  IEnumerator DamageOverTime()
{
    GameManager.Instance.TakeDamage(damage); // โดนทันที

    while (true)
    {
        yield return new WaitForSeconds(damageInterval);
        GameManager.Instance.TakeDamage(damage);
    }
}

}
