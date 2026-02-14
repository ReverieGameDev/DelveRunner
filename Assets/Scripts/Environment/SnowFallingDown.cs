using UnityEngine;

public class SnowFallingDown : MonoBehaviour
{
    float verticalFallSpeed;
    float horizontalFallSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        verticalFallSpeed = Random.Range(-6f, -2f);
        horizontalFallSpeed = Random.Range(-1f, -0.09f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(horizontalFallSpeed * Time.deltaTime, verticalFallSpeed * Time.deltaTime, 0);
        if (transform.position.y < -47)
        {
            Destroy(gameObject);
        }
    }
}
