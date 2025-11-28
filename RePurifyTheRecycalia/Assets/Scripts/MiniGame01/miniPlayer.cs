using UnityEngine;
using UnityEngine.InputSystem;

public class miniPlayer : MonoBehaviour
{
    public float speed = 5f;

    public float leftLimit = -8f;
    public float rightLimit = 8f;

    void Update()
    {
        // เช็คว่าเกมเริ่มหรือยัง
        if (!MiniGame01.Instance.gameStarted)
            return;

        float move = 0f;

        if (Keyboard.current.aKey.isPressed) move = -1f;
        if (Keyboard.current.dKey.isPressed) move = 1f;

        // เดิน
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);

        // wrap-around
        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > rightLimit)
        {
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        TrashItem item = col.GetComponent<TrashItem>();
        if (item == null) return;

        if (item.trashType == MiniGame01.Instance.targetTrashType)
        {
            MiniGame01.Instance.AddScore(100);
        }
        else
        {
            MiniGame01.Instance.LoseLife();
        }

        Destroy(col.gameObject);
    }
}
