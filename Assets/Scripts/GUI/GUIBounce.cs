using System.Collections;
using UnityEngine;

public class GUIBounce : MonoBehaviour
{
    private float startY;
    private float bottomY;
    private float bounceDistance = 0.08f; // 0.002 * 40

    void Start()
    {
        startY = transform.position.y;
        bottomY = startY - bounceDistance;
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        // Go down
        for (int i = 0; i < 40; i++)
        {
            float t = i / 40f;
            float y = Mathf.Lerp(startY, bottomY, t);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return new WaitForSeconds(.02f);
        }

        // Go up
        for (int i = 0; i < 40; i++)
        {
            float t = i / 40f;
            float y = Mathf.Lerp(bottomY, startY, t);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return new WaitForSeconds(.02f);
        }

        StartCoroutine(Bounce());
    }
}