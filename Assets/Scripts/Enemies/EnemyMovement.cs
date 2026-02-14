using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Enemy enemy;
    private Vector2 enemyPosition;
    private PlayerMovement playerMovement;
    

    void Start()
    {
        enemy = GetComponent<Enemy>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        
    }

    void FixedUpdate()
    {
        float enemySpeed = enemy.enemySpeed;
        enemyPosition = transform.position;
        transform.Translate(((playerMovement.playerPosition - enemyPosition).normalized) * enemySpeed * Time.fixedDeltaTime);
    }
}