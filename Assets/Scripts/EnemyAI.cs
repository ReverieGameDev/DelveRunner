using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState;
    private Transform player;
    private Rigidbody2D rb;
    public float speed = .5f;
    private SpawnManager spawnManager;
    private Vector2 centerFormationTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        currentState = EnemyState.Chase;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        centerFormationTile = spawnManager.spawnPos;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Death:
                Death();
                break;

            case EnemyState.Reposition:
                Reposition();
                break;

            case EnemyState.Retreat:
                Retreat();
                break;
        }
    }

    private void Attack()
    {
        if (Vector2.Distance((Vector2)transform.position, player.position) >= 18)
        {
            currentState = EnemyState.Chase;
        }
    }
    private void Chase()
    {
        Vector2 chaseDirection = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
        rb.position = ((Vector2)transform.position + (chaseDirection * speed * Time.deltaTime));
        if (Vector2.Distance((Vector2)transform.position,player.position) <= 12)
        {
            currentState = EnemyState.Attack;
        }
    }
    private void Death()
    {
        //play death anim
        Destroy(gameObject);
    }
    private void Reposition()
    {

    }
    private void Retreat()
    {

    }
}
