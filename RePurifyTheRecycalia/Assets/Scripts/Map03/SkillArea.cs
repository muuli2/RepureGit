using UnityEngine;

public class SkillArea : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // หายหลังอนิเมเล่นเสร็จ
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monster"))
        {
            Monster m = col.GetComponent<Monster>();
            if (m != null)
                m.TakeDamage((int)damage);
        }
    }
}
