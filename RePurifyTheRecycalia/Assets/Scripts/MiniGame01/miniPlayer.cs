using UnityEngine;
using UnityEngine.InputSystem;

public class miniPlayer : MonoBehaviour
{
    public float speed = 5f;
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    void Update()
    {
        if (!MiniGame01.Instance.gameStarted)
            return;

        float move = 0f;

        if (Keyboard.current.aKey.isPressed) move = -1f;
        if (Keyboard.current.dKey.isPressed) move = 1f;

        transform.Translate(Vector3.right * move * speed * Time.deltaTime);

        if (transform.position.x < leftLimit)
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        else if (transform.position.x > rightLimit)
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        TrashItem item = col.GetComponent<TrashItem>();
        if (item == null) return;

        if (item.trashType == MiniGame01.Instance.targetTrashType)
        {
            MiniGame01.Instance.AddScore(100);

            // 🔊 เล่นเสียงเก็บถูก
            if (correctSFX != null)
                audioSource.PlayOneShot(correctSFX);
        }
        else
        {
            GameManager.Instance.TakeDamage(1);
            MiniGame01.Instance.UpdateHeartsUI();

            // 🔊 เล่นเสียงเก็บผิด
            if (wrongSFX != null)
                audioSource.PlayOneShot(wrongSFX);

            if (GameManager.Instance.lives <= 0)
                MiniGame01.Instance.GameOver();
        }

        Destroy(col.gameObject);
    }
}
