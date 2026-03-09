using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState;
    private Transform player;
    private Rigidbody2D rb;
    private float speed = 8f;
    private SpawnManager spawnManager;
    public GameObject assignedSpawnAnchor;
    public EnemyRoles role;

    // Offset and rotation
    private Vector2 positionOffset;
    private Vector2 originalOffset;
    private float offsetDistance;
    private float initialAngle;

    // Movement helpers
    private Vector2 anchorPlayerAngle;
    private Vector2 currentPos;
    private Vector2 anchorPos;
    private Vector2 anchorDirection;

    // Retreat
    private Vector2 directionToRetreat;
    private Vector2 retreatStartPos;
    private bool hasStartedRetreating = false;
    public bool isCharging;

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.Attack;

        positionOffset = new Vector2(transform.position.x - assignedSpawnAnchor.transform.position.x, transform.position.y - assignedSpawnAnchor.transform.position.y);
        originalOffset = positionOffset;
        offsetDistance = positionOffset.magnitude;

        Vector2 toPlayer = (Vector2)player.position - (Vector2)assignedSpawnAnchor.transform.position;
        initialAngle = Mathf.Atan2(toPlayer.y, toPlayer.x);
    }

    void FixedUpdate()
    {
        anchorPlayerAngle = ((Vector2)player.position - (Vector2)assignedSpawnAnchor.transform.position).normalized;

        // Rotate offset when player flanks past threshold
        float currentAngle = Mathf.Atan2(anchorPlayerAngle.y, anchorPlayerAngle.x);
        float angleDiff = currentAngle - initialAngle;

        if (Mathf.Abs(angleDiff) > 0.65f)
        {
            originalOffset = new Vector2(
                originalOffset.x * Mathf.Cos(angleDiff) - originalOffset.y * Mathf.Sin(angleDiff),
                originalOffset.x * Mathf.Sin(angleDiff) + originalOffset.y * Mathf.Cos(angleDiff)
            );
            initialAngle = currentAngle;
        }

        positionOffset = originalOffset;

        // State machine
        switch (currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Death:
                Death();
                break;
            case EnemyState.Retreat:
                Retreat();
                break;
        }

        // Follow anchor + offset
        if (!isCharging)
        {
            currentPos = transform.position;
            anchorPos = assignedSpawnAnchor.transform.position;
            if (Vector2.Distance(currentPos, anchorPos + positionOffset) >= .25f)
            {
                anchorDirection = new Vector2(anchorPos.x + positionOffset.x - currentPos.x, anchorPos.y + positionOffset.y - currentPos.y).normalized;
                rb.MovePosition((Vector2)transform.position + anchorDirection * speed * Time.fixedDeltaTime);
            }
        }

    }


    private void Attack()
    {
        switch (role)
        {
            case EnemyRoles.Archer:
                if (Vector2.Distance((Vector2)transform.position, player.position) <= 5f)
                {
                    currentState = EnemyState.Retreat;
                }
                break;
            case EnemyRoles.Summoner:
                if (Vector2.Distance((Vector2)transform.position, player.position) <= 5f)
                {
                    currentState = EnemyState.Retreat;
                }
                break;
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void Retreat()
    {
        if (hasStartedRetreating == false)
        {
            retreatStartPos = transform.position;
            hasStartedRetreating = true;
        }
        directionToRetreat = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.MovePosition((Vector2)transform.position + directionToRetreat * speed * Time.fixedDeltaTime);
        if (Vector2.Distance(retreatStartPos, transform.position) >= 7)
        {
            currentState = EnemyState.Attack;
            hasStartedRetreating = false;
        }
    }
}