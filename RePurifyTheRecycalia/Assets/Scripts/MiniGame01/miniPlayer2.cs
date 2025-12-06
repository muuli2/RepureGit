using UnityEngine;
using UnityEngine.InputSystem;

public class miniPlayer2 : MonoBehaviour
{
    public float speed = 5f;
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    void Update()
    {
        if (!MiniGame012.Instance.gameStarted)
            return;

        float move = 0f;

        if (Keyboard.current.aKey.isPressed) move = -1f;
        if (Keyboard.current.dKey.isPressed) move = 1f;

        transform.Translate(Vector3.right * move * speed * Time.deltaTime);

        // wrap-around
        if (transform.position.x < leftLimit)
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        else if (transform.position.x > rightLimit)
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
    }

   void OnTriggerEnter2D(Collider2D col)
{
    TrashItem2 item = col.GetComponent<TrashItem2>();
    if (item == null) return;

    // ✅ ต้องใช้ item.trashType2 (ตัวเล็ก)
    if (item.trashType2 == MiniGame012.Instance.targetTrashType)
    {
        MiniGame012.Instance.AddScore(100);
    }
    else
    {
        GameManager.Instance.TakeDamage(1);
        MiniGame012.Instance.UpdateHeartsUI();

        if (GameManager.Instance.lives <= 0)
            MiniGame012.Instance.GameOver();
    }

    Destroy(col.gameObject);
}


}
