using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DropData
{
    public GameObject item;
    public float dropChance; // 0-100%
}

public class Monster : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    public Image healthBarFill;

    [Header("Drop Settings")]
    public DropData[] drops;

    [Header("Attack Settings")]
    public int damageToPlayer = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    [Header("Score Settings")]
    public int scoreOnDeath = 150;

    [Header("SFX")]
public AudioSource audioSource;
public AudioClip dieClip;


    private Vector3 startPosition;
    private Animator anim;
    private Collider2D col;

    void Awake()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
        UpdateHealthBar();

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth > 0)
        {
            if (anim != null)
                anim.SetTrigger("hurt");
        }
        else
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

   void Die()
{
    if (audioSource && dieClip)
        audioSource.PlayOneShot(dieClip);

    if (anim != null)
        anim.SetTrigger("die");

    if (col != null)
        col.enabled = false;

    StartCoroutine(DieRoutine());
}


    IEnumerator DieRoutine()
    {
        // รออนิเมชันตาย (แก้ตามความยาวคลิป)
        yield return new WaitForSeconds(0.6f);

        // สุ่มดรอปไอเท็ม
        foreach (var d in drops)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= d.dropChance)
                Instantiate(d.item, transform.position, Quaternion.identity);
        }

        // เพิ่มคะแนน
        ScoreManage.Instance?.AddScore(scoreOnDeath);
        MonsterManage.Instance.EnemyKilled();

        // ปิดมอนสเตอร์
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            TryDamagePlayer();
    }

    void TryDamagePlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            GameManager.Instance.TakeDamage(damageToPlayer);
        }
    }

    public void ResetMonster()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        gameObject.SetActive(true);
        lastAttackTime = 0f;

        transform.position = startPosition;

        // เปิด collider กลับ
        if (col != null)
            col.enabled = true;

        // รีเซ็ตอนิเมชันให้กลับ Idle
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }
}
