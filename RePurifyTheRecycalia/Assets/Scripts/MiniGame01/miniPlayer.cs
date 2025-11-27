using UnityEngine;

public class miniPlayer : MonoBehaviour
{
    public float speed = 5f;
    private float move;

    void Update()
    {
        // Move Input A D / Left Right
        move = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        TrashItem item = col.GetComponent<TrashItem>();
        if (item == null) return;

        if (item.trashType == MiniGame01.Instance.targetTrashType)
        {
            // ถ้าถูกประเภท
            MiniGame01.Instance.AddScore(100);
        }
        else
        {
            // ถ้าผิดประเภท
            MiniGame01.Instance.LoseLife();
        }

        Destroy(col.gameObject);
    }
}
