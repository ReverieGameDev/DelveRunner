using UnityEngine;

public class FormationAnchorBehaviour : MonoBehaviour
{
    private float speed = 4f;
    private Vector2 chaseDirection;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance((Vector2)transform.position, player.position) >= 16)
        {
            chaseDirection = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
            transform.position = ((Vector2)transform.position + (chaseDirection ));
        }
    }
}

