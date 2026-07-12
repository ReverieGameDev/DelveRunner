using System;
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
    public FormationAnchorBehaviour assignedSpawnAnchorScript;
    private FormationAnchorBehaviour anchorBehaviour;   // cached once, read every tick
    public EnemyRoles role;
    private Animator anim;
    public bool animOverride = false;
    public EnemyMode currentMode = EnemyMode.Formation;
    private bool isSolo = false;
    

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
        StartCoroutine(EnemyHeartbeat());
    }
    IEnumerator EnemyHeartbeat()
    {
        int stableOffset = 2;
        int decisionOffset = UnityEngine.Random.Range(0, 6);

        if (assignedSpawnAnchorScript.formationBroken && currentMode == EnemyMode.Formation)
        {
            yield return new WaitForSeconds(decisionOffset);
            currentMode = EnemyMode.Solo;
            //if other conditions are met, then we              currentMode = EnemyMode.Decide; and figure out which we switch to
        }
        yield return new WaitForSeconds(stableOffset);
        if (currentMode != EnemyMode.Solo)
        {
            StartCoroutine(EnemyHeartbeat());
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
                Solo();
                break;
            case EnemyMode.Environment:
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
        if (!isSolo)
        {
            StartCoroutine(EnemySolo());
        }
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