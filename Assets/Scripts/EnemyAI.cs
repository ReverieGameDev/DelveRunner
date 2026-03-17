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
    private Animator anim;
    public bool animOverride = false;

    // Movement helpers
    private Vector2 anchorPlayerAngle;
    private Vector2 anchorPos;
    private float initialAngle;

    // Retreat
    private Vector2 directionToRetreat;
    private Vector2 retreatStartPos;
    private bool hasStartedRetreating = false;
    public bool isCharging;

    // Ring formation
    private int originalRingIndex;
    private int currentRingIndex;
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
        anim = GetComponent<Animator>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.Attack;
        anchorPos = assignedSpawnAnchor.transform.position;

        Vector2 spawnOffset = new Vector2(
            Mathf.Round(transform.position.x - anchorPos.x),
            Mathf.Round(transform.position.y - anchorPos.y)
        );

        if (spawnOffset == Vector2.zero)
        {
            isCenter = true;
        }
        else
        {
            for (int i = 0; i < ringOrder.Length; i++)
            {
                if (spawnOffset == ringOrder[i])
                {
                    positionInRingOrder = i;
                    break;
                }
            }
        }
        originalRingIndex = positionInRingOrder;
        currentRingIndex = positionInRingOrder;
        Vector2 toPlayer = (Vector2)player.position - anchorPos;
        initialAngle = Mathf.Atan2(toPlayer.y, toPlayer.x);
    }

    void FixedUpdate()
    {
        anchorPos = assignedSpawnAnchor.transform.position;
        anchorPlayerAngle = ((Vector2)player.position - anchorPos).normalized;

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

        if (!isCharging)
        {
            Vector2 targetPos;
            if (isCenter)
            {
                targetPos = anchorPos;
            }
            else
            {
                float currentAngle = Mathf.Atan2(anchorPlayerAngle.y, anchorPlayerAngle.x);
                float angleDiff = Mathf.DeltaAngle(initialAngle * Mathf.Rad2Deg, currentAngle * Mathf.Rad2Deg);
                int rotationSteps = Mathf.RoundToInt(angleDiff / 45f);
                int targetRingIndex = ((originalRingIndex - rotationSteps) % 8 + 8) % 8;

                targetPos = anchorPos + ringOrder[currentRingIndex];

                if (Vector2.Distance((Vector2)transform.position, targetPos) < 0.5f && currentRingIndex != targetRingIndex)
                {
                    int diff = (targetRingIndex - currentRingIndex + 8) % 8;
                    if (diff <= 4)
                        currentRingIndex = (currentRingIndex + 1) % 8;
                    else
                        currentRingIndex = (currentRingIndex + 7) % 8;
                }
            }

            if (Vector2.Distance((Vector2)transform.position, targetPos) >= 0.5f)
            {
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                transform.position = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
                SetWalkAnim(1);
            }
            else
            {
                transform.position = (Vector3)targetPos;
                SetWalkAnim(0);
            }
        }
    }

    private void SetWalkAnim(int value)
    {
        if (animOverride) return;

        switch (role)
        {
            case EnemyRoles.Archer:
                anim.SetInteger("ArcherInt", value);
                break;
            case EnemyRoles.Warrior:
                anim.SetInteger("WarriorInt", value);
                break;
            case EnemyRoles.Summoner:
                anim.SetInteger("NecromancerInt", value);
                break;
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
        speed = 0;
    }

    private void Retreat()
    {
        SetWalkAnim(1);
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