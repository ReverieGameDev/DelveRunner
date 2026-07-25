using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState;
    private Transform player;
    private Rigidbody2D rb;
    private float speed = 8f;
    private SpawnManager spawnManager;
    public GameObject assignedSpawnAnchor;
    public FormationAnchorBehaviour assignedSpawnAnchorScript;
    private FormationAnchorBehaviour anchorBehaviour;   // cached once, read every tick
    public EnemyRoles role;
    private Animator anim;
    public bool animOverride = false;
    public EnemyMode currentMode = EnemyMode.Formation;
    private bool isSolo = false;
    public SoloSquares[] soloSquaresPrefab;
    public EnemySoloState enemySoloState = EnemySoloState.None;
    private SoloSquares bestSquare;
    private SoloSquares currentSquare;
    private Vector2 soloTarget;
    private Enemy enemy;
    private List<EnemyAI> allEnemies = new List<EnemyAI>();
    public bool isAttacking = false;
    public bool walkingToET = false;
    private EnvironmentThreat environmentThreat;


    // Movement helpers
    private Vector2 anchorPos;

    // Retreat
    private Vector2 directionToRetreat;
    private Vector2 retreatStartPos;
    private bool hasStartedRetreating = false;
    public bool isCharging;
    public bool isBackline = false;
    public bool backlineDead = false;
    public bool isFrontline = false;
    public bool isDead = false;


    //solo
    public bool isMovingSolo = false;

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
        environmentThreat = FindFirstObjectByType<EnvironmentThreat>();
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        if (isFrontline)
        {
            Enemy enemy = GetComponent<Enemy>();
            
        }
        currentState = EnemyState.Attack;
        anchorBehaviour = assignedSpawnAnchor.GetComponent<FormationAnchorBehaviour>();
        anchorPos = assignedSpawnAnchor.transform.position;
        if (isBackline) { anchorBehaviour.backlineEnemiesLeftAlive++; }

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
        soloTarget = transform.position;
        StartCoroutine(EnemyHeartbeat());
    }
    IEnumerator EnemyHeartbeat()
    {
        // one-time stagger so enemies stay desynced
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 6f));
        while (true)
        {

            if (assignedSpawnAnchorScript.formationBroken && currentMode == EnemyMode.Formation)
            {
                currentMode = EnemyMode.Solo;
            }
            if (currentMode == EnemyMode.Solo)
            {
                if (Vector2.Distance(player.transform.position, transform.position) <= 20 && currentMode == EnemyMode.Solo && (enemySoloState == EnemySoloState.None || enemySoloState == EnemySoloState.soloIsIdle))
                {
                float currentBestCandidate = Mathf.Infinity;
                bestSquare = null;
                foreach (SoloSquares moveableSquare in soloSquaresPrefab)
                {
                    if (currentSquare == null) //if the formation has just broken, we do not make a "closest best option" move for the enemy
                    {
                        if (!moveableSquare.squareOccupied && Vector2.Distance(moveableSquare.transform.position, player.transform.position) > 12f)
                        {
                            if (Vector2.Distance(moveableSquare.transform.position, player.transform.position) < currentBestCandidate)
                            {
                                bestSquare = moveableSquare;
                                currentBestCandidate = Vector2.Distance(moveableSquare.transform.position, player.transform.position);
                            }
                        }
                    }
                    if (currentSquare != null) //if the enemy's formation has already broken and it has alreayd moved to the square, to prevent the enemy from needlessly moving opposite to the player
                                               //, we check for the distance betweencurrent and the square in question
                    {
                        if (!moveableSquare.squareOccupied && Vector2.Distance(moveableSquare.transform.position, player.transform.position) > 12f && Vector2.Distance(moveableSquare.transform.position, currentSquare.transform.position) < 8f)
                        {
                            if (Vector2.Distance(moveableSquare.transform.position, player.transform.position) < currentBestCandidate)
                            {
                                bestSquare = moveableSquare;
                                currentBestCandidate = Vector2.Distance(moveableSquare.transform.position, player.transform.position);
                            }
                        }
                    }
                }
                if (bestSquare != currentSquare && currentSquare != null)
                {
                    currentSquare.squareOccupied = false;
                }
                if (bestSquare != null)
                {
                    bestSquare.squareOccupied = true;
                    soloTarget = bestSquare.transform.position;
                    currentSquare = bestSquare;
                }
                if (bestSquare == null) //if the enemy is stuck in a corner/didn't find a square to move to despite being prompted to do so:
                {
                    currentBestCandidate = Mathf.Infinity;
                    foreach (SoloSquares moveableSquare in soloSquaresPrefab)
                    {

                        if (!moveableSquare.squareOccupied && moveableSquare != currentSquare && Vector2.Distance(moveableSquare.transform.position,currentSquare.transform.position) > 12)
                        {
                            float d = Vector2.Distance(moveableSquare.transform.position, currentSquare.transform.position);
                            if (d < currentBestCandidate)     // ← THIS is what's missing
                            {
                                bestSquare = moveableSquare;
                                currentBestCandidate = d;
                            }
                        }
                    }
                    if (bestSquare != null)
                    {
                        if (currentSquare != null) currentSquare.squareOccupied = false;
                        bestSquare.squareOccupied = true;
                        soloTarget = bestSquare.transform.position;
                        currentSquare = bestSquare;
                    }
                }
            }

            }
            if (currentMode == EnemyMode.Environment)
            {
                if (!walkingToET)
                {
                    //code for animations, chanting.
                }
            }
            yield return new WaitForSeconds(TickForMode());
        }
    }

    float TickForMode()
    {
        switch (currentMode)
        {
            case EnemyMode.Formation: return 4f;
            case EnemyMode.Solo: return UnityEngine.Random.Range(2,5);
            case EnemyMode.Environment: return 1f;
            case EnemyMode.Decide: return 0.5f;
            default: return 2f;
        }
    }

    IEnumerator EnemySolo()
    {
        transform.position = new Vector3(player.position.x , player.position.y+3);
        yield return new WaitForSeconds(2f);


    }

    public void ReduceFromBackline()
    {
        anchorBehaviour.backlineEnemiesLeftAlive--;
        if (anchorBehaviour.backlineEnemiesLeftAlive == 0)
        {
            anchorBehaviour.canWarriorLeap = true;
        }
    }

    void FixedUpdate()
    {

        if (currentState == EnemyState.Death && !isDead)
        {
            isDead = true;
            Death();
            
        }
        if (isDead) return;

        switch (currentMode)
        {
            case EnemyMode.Formation:
                FormationMode();
                break;
            case EnemyMode.Decide:
                Decide();
                return;
            case EnemyMode.Solo:
                if (Vector2.Distance(transform.position, soloTarget) > .1f) enemySoloState = EnemySoloState.soloIsMoving; // if the enemy has not reached their destination
                else enemySoloState = EnemySoloState.soloIsIdle; // if the enemy has not reached their destination
                if (enemySoloState == EnemySoloState.soloIsMoving) SetWalkAnim(1);
                if (enemySoloState == EnemySoloState.soloIsIdle) SetWalkAnim(0);
                Solo();
                rb.MovePosition(Vector2.MoveTowards(rb.position, soloTarget, speed * Time.fixedDeltaTime));
                break;
            case EnemyMode.Environment:
                if (Vector2.Distance(transform.position, environmentThreat.operatorCoords) > .1f) //operator walks towards the destination to begin interacting with environment
                {
                    rb.MovePosition(Vector2.MoveTowards(rb.position, environmentThreat.operatorCoords, speed * Time.fixedDeltaTime));
                    SetWalkAnim(1);
                }
                
                else if (walkingToET) // if the operator reaches the destination, they are no longer walking there.
                {
                    walkingToET = false;
                    environmentThreat.EnvironmentOperator(this);
                    SetWalkAnim(0); //actually the chant anim, havent made it yet
                } 
                Environment();
                break;
        }
        switch (currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void FormationMode()
    {
        
        if (anchorBehaviour == null || anchorBehaviour.canWarriorLeap == true) return;

        anchorPos = assignedSpawnAnchor.transform.position;
        Vector2 targetPos;
            if (isCenter)
            {
                targetPos = anchorPos;
            }
            else
            {
                // the anchor decides rotation for the whole formation - we just follow it
                int targetRingIndex = ((originalRingIndex - anchorBehaviour.totalRotationSteps) % 8 + 8) % 8;

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
                rb.MovePosition((Vector2)transform.position + direction * speed * Time.fixedDeltaTime);
                SetWalkAnim(1);
            }
            else
            {
                rb.MovePosition(targetPos);
                SetWalkAnim(0);
            }
    }
    private void Decide()
    {
        currentMode = EnemyMode.Solo;
    }
    private void Environment()
    {

    }
    private void Solo()
    {
        isSolo = true;
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

    }

    private void Death()
    {
        speed = 0;
    }

}