using Unity.VisualScripting;
using UnityEngine;

public class SummonerCat : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 10;
    private PlayerMovement playerMovement;
    private Vector2 catToPlayerVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        catToPlayerVector = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
        rb.MovePosition((Vector2)transform.position + catToPlayerVector * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
