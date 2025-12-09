using UnityEngine;

public class FinalBossBullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public float lifeTime = 5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
