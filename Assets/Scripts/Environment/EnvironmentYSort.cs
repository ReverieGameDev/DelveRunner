using UnityEngine;

public class EnvironmentYSort : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)((32000) - (transform.position.y * 100));
    }
}