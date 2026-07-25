using JetBrains.Annotations;
using System.Collections;
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
        Interrupted
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        operatorCoords = transform.position;
        spawnManager = FindFirstObjectByType<SpawnManager>();
        switch (currentEnvironmentThreatName)
        {
            case EnvironmentThreatName.Zapper:
                    totalChargeTime = 10f;
                    cooldownTime = 4f;
                    interruptPenaltyTime = 2f;
                cooldownAfterFire = 4f;
                needOperator = true;
                break;
            case EnvironmentThreatName.HealingTotem:
                    totalChargeTime = 5f;
                    cooldownTime = 5f;
                    interruptPenaltyTime = 5f;
                cooldownAfterFire = 3f;
                needOperator = false;
                break;
            case EnvironmentThreatName.NullObelisk:
                    totalChargeTime = 8f;
                    cooldownTime = 6f;
                    interruptPenaltyTime = 2f;
                cooldownAfterFire = 2f;
                needOperator = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needOperator && enemyOperator == null && environmentState != EnvironmentState.Idle)
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
                    if (chargeCounter >= totalChargeTime)
                    {
                        environmentState = EnvironmentState.Firing;
                        stateTimer = cooldownAfterFire;
                    }
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
            }
        }
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

            if (enemy.enemyHealth / enemy.maxEnemyHealth < 0.75f) continue;
            if (enemyAI.isAttacking) continue;
            if (enemyAI.walkingToET) continue;

            validOperators.Add(enemyObject);
        }

        if (validOperators.Count == 0) return;
        walkingOperator = validOperators[Random.Range(0, validOperators.Count)];
        walkingOperator.GetComponent<EnemyAI>().currentMode = EnemyMode.Environment;
        walkingOperator.GetComponent<EnemyAI>().walkingToET = true;
        Debug.Log("Looking for operator, found: " + walkingOperator);
    }
}

