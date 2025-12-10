using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour
{
    public float panDistance = 2f;
    public float panDuration = 2f;

    public IEnumerator PanUp()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, panDistance, 0);

        float t = 0;
        while (t < panDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / panDuration);
            yield return null;
        }
    }
}
