
using System;
using System.Collections.Generic;
using UnityEngine;
using static EnvironmentThreat;

public class EnvironmentThreat : MonoBehaviour
{
    private float totalChargeTime;
    private float cooldownTime;
    private float interruptPenaltyTime;
    private float cooldownAfterFire;
    private EnemyAI enemyOperator;
    private bool needOperator;
    private bool searchingForOperator = true;
    private GameObject walkingOperator;
    private SpawnManager spawnManager;
    private List<GameObject> validOperators = new List<GameObject>();
    public Vector2 operatorCoords;
    public Transform barFill;
    private Vector3 barStartScale;
    private float deathTime;
    public float zapperDamage = 20f;
    public float healingTotemHeal = 35f;
    public GameObject nullCircle;
    private float hitCooldown;


    public SpriteRenderer etColor; 

    private float chargeCounter;
    private float stateTimer;
    public EnvironmentState environmentState = EnvironmentState.Idle;
    public EnvironmentThreatName currentEnvironmentThreatName;
    
    public enum EnvironmentThreatName
    {
        Zapper,
        HealingTotem,
        NullObelisk
    }
    public enum EnvironmentState
    {
        Idle,
        Charging,
        Firing,
        Interrupted,
        OperatorDeath
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        etColor = barFill.GetComponent<SpriteRenderer>();
        barStartScale = barFill.localScale;
        operatorCoords = transform.position;
        spawnManager = FindFirstObjectByType<SpawnManager>();
        switch (currentEnvironmentThreatName)
        {
            case EnvironmentThreatName.Zapper:
                    totalChargeTime = 10f;
                    cooldownTime = 4f;
                    interruptPenaltyTime = 2f;
                    deathTime = 5f;
                
                cooldownAfterFire = 4f;
                needOperator = true;
                break;
            case EnvironmentThreatName.HealingTotem:
                    totalChargeTime = 5f;
                    cooldownTime = 5f;
                    interruptPenaltyTime = 5f;
                cooldownAfterFire = 3f;
                deathTime = 0f;
                hitCooldown = 1f;
                needOperator = false;
                break;
            case EnvironmentThreatName.NullObelisk:
                    totalChargeTime = 8f;
                    cooldownTime = 6f;
                    interruptPenaltyTime = 2f;
                cooldownAfterFire = 2f;
                deathTime = 7f;
                needOperator = true;
                hitCooldown = 1f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needOperator && enemyOperator == null && (environmentState == EnvironmentState.Charging || environmentState == EnvironmentState.Firing))
        {
            environmentState = EnvironmentState.Idle;
        }
        else
        {
            switch (environmentState)
            {
                case EnvironmentState.Idle: //ET is unoccupied, idle.
                    if (!needOperator) { environmentState = EnvironmentState.Charging; break; }
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0f && walkingOperator == null && enemyOperator == null)
                    {
                        Debug.Log("firing find operator");
                        stateTimer = 3f;
                        FindOperator();
                    }
                    break;
                case EnvironmentState.Charging: //ET is occupied by an enemy, is charging
                    chargeCounter += Time.deltaTime;
                    hitCooldown = MathF.Max(hitCooldown - Time.deltaTime, 0);
                    if (chargeCounter >= totalChargeTime)
                    {
                        switch (currentEnvironmentThreatName)
                        {
                            case EnvironmentThreatName.Zapper:
                                EnvironmentThreatZapper();

                                break;
                            case EnvironmentThreatName.HealingTotem:
                                EnvironmentThreatHealingTotem();
                                break;
                            case EnvironmentThreatName.NullObelisk:
                                EnvironmentThreatNullObelisk();
                                break;
                        }
                        environmentState = EnvironmentState.Firing;
                        stateTimer = cooldownAfterFire;
                    }
                    barFill.localScale = new Vector3(barStartScale.x * EnvironmentChargePercent(), barStartScale.y, barStartScale.z);
                    break;
                case EnvironmentState.Firing: //ET has reached the total charge, fires.
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        environmentState = EnvironmentState.Charging;
                        chargeCounter = 0;

                    }
                    break;
                case EnvironmentState.Interrupted: //ET is interrupted by a stun
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        environmentState = EnvironmentState.Charging;
                    }
                    break;
                case EnvironmentState.OperatorDeath: //ET is interrupted by death
                    stateTimer -= Time.deltaTime;
                    if (chargeCounter >= 0) chargeCounter -= Time.deltaTime;
                    barFill.localScale = new Vector3(barStartScale.x * EnvironmentChargePercent(), barStartScale.y, barStartScale.z);
                    if (stateTimer <= 0)
                    {
                        environmentState = EnvironmentState.Idle;
                    }
                    break;
            }
        }
    }
    public void OperatorHasDied()
    {
        environmentState = EnvironmentState.OperatorDeath;
        stateTimer = deathTime;
        enemyOperator = null;
        walkingOperator = null;
    }
    public void InterruptEnvironment()
    {
        if (environmentState == EnvironmentState.Firing) return;
        chargeCounter = Mathf.Max(0,chargeCounter - interruptPenaltyTime);
        stateTimer = cooldownTime;
        environmentState = EnvironmentState.Interrupted;
    }
    public float EnvironmentChargePercent()
    {
        return chargeCounter / totalChargeTime;
    }

    public void EnvironmentOperator(EnemyAI enemy) //ONLY SEND THIS ONCE THE OPERATOR GETS THERE!
    {
        enemyOperator = enemy;
        environmentState = EnvironmentState.Charging;
    }

    private void FindOperator()
    {
        validOperators.Clear();

        foreach (GameObject enemyObject in spawnManager.spawnedEnemies)
        {
            if (enemyObject == null) continue;

            Enemy enemy = enemyObject.GetComponent<Enemy>();
            EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

            if (enemy.enemyHealth / enemy.maxEnemyHealth < 0.5f) continue;
            //if (enemyAI.isAttacking) continue;
            if (enemyAI.walkingToET) continue;
            if (enemyAI.currentMode != EnemyMode.Solo) continue;
            validOperators.Add(enemyObject);
        }

        if (validOperators.Count == 0) return;
        walkingOperator = validOperators[UnityEngine.Random.Range(0, validOperators.Count)];
        walkingOperator.GetComponent<EnemyAI>().currentMode = EnemyMode.Environment;
        walkingOperator.GetComponent<EnemyAI>().walkingToET = true;
        walkingOperator.GetComponent<EnemyAI>().environmentThreat = this;
    }

    public void EnvironmentThreatZapper()
    {
        PlayerCombat.Instance.DamagePlayer(zapperDamage);
    }

    public void EnvironmentThreatHealingTotem()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            enemy.GetComponent<Enemy>().HealEnemy(healingTotemHeal);
        }
    }

    public void EnviromentThreatHealingTotemReduceTime()
    {
        if (environmentState == EnvironmentState.Charging)
        {
            chargeCounter = Mathf.Max(chargeCounter - 1,0);
        }
    }
    public void EnvironmentThreatNullObelisk()
    {
        nullCircle.transform.localScale += new Vector3(5f, 5f, 0);
    }
    public void EnvironmentThreatReduceNullObelisk()
    {
        nullCircle.transform.localScale += new Vector3(-1f, -1f, 0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            switch (currentEnvironmentThreatName)
            {
                case EnvironmentThreatName.Zapper:

                    break;
                case EnvironmentThreatName.HealingTotem:
                    if (hitCooldown <= 0)
                    {
                        EnviromentThreatHealingTotemReduceTime();
                        hitCooldown = 1f;
                    }
                    break;
                case EnvironmentThreatName.NullObelisk:
                    if (hitCooldown <= 0)
                    {
                        EnvironmentThreatReduceNullObelisk();
                        hitCooldown = 1f;
                    }
                    
                    break;
            }
        }
    }
}

