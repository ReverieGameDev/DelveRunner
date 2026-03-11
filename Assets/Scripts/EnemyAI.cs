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
    private Vector2 targetPos;
    private float targetAngle;

    // Retreat
    private Vector2 directionToRetreat;
    private Vector2 retreatStartPos;
    private bool hasStartedRetreating = false;
    public bool isCharging;

    // Ring formation
    private int positionInRingOrder;
    private bool isCenter = false;
    private Vector2[] ringOrder = {
        new Vector2(-3,3),
        new Vector2(0,3),
        new Vector2(3,3),
        new Vector2(3,0),
        new Vector2(3,-3),
        new Vector2(0,-3),
        new Vector2(-3,-3),
        new Vector2(-3,0)
    };

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.Attack;
        anchorPos = assignedSpawnAnchor.transform.position;

        // Figure out which ring slot this enemy spawned in
        Vector2 spawnOffset = new Vector2(
            Mathf.Round(transform.position.x - anchorPos.x),
            Mathf.Round(transform.position.y - anchorPos.y)
        );

        // Check if this enemy is in the center (like the archer)
        if (spawnOffset == Vector2.zero)
        {
            isCenter = true;
        }
        else
        {
            // Find matching ring position
            for (int i = 0; i < ringOrder.Length; i++)
            {
                if (spawnOffset == ringOrder[i])
                {
                    positionInRingOrder = i;
                    break;
                }
            }
        }

        // Save starting angle from anchor to player
        Vector2 toPlayer = (Vector2)player.position - anchorPos;
        initialAngle = Mathf.Atan2(toPlayer.y, toPlayer.x);
    }

    void FixedUpdate()
    {
        anchorPos = assignedSpawnAnchor.transform.position;
        anchorPlayerAngle = ((Vector2)player.position - anchorPos).normalized;

        // Rotate offset when player flanks past threshold
        float currentAngle = Mathf.Atan2(anchorPlayerAngle.y, anchorPlayerAngle.x);
        float angleDiff = Mathf.DeltaAngle(initialAngle * Mathf.Rad2Deg, currentAngle * Mathf.Rad2Deg);

        if (!isCenter)
        {
            if (angleDiff > 45f)
            {
                initialAngle = currentAngle;
                positionInRingOrder = (positionInRingOrder + 7) % 8; // was +1
            }
            else if (angleDiff < -45f)
            {
                initialAngle = currentAngle;
                positionInRingOrder = (positionInRingOrder + 1) % 8; // was +7
            }
        }
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
        // Move toward assigned ring position (or center)
        if (!isCharging)
        {
            Vector2 targetPos;
            if (isCenter)
            {
                targetPos = anchorPos;
            }
            else
            {
                targetPos = anchorPos + ringOrder[positionInRingOrder];
            }

            if (Vector2.Distance((Vector2)transform.position, targetPos) >= 0.25f)
            {
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                transform.position = ((Vector2)transform.position + direction * speed * Time.fixedDeltaTime);
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