using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TankSkeleton : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject slashAttackPrefab;
    public GameObject slashAttackIndicator;
    public Sprite slashIcon;
    public Sprite regenerateIcon;
    public Sprite chargeIcon;
    private EnemyAttackIndicator enemyAttackIndicator;
    private List<string> attackList = new List<string>();
    private bool isReadyToAttack = true;
    private string currentAttack;
    private float attackWindupTime;
    private bool slashCheckCooldownBool = false;
    private bool regenerationCheckCooldownBool = false;
    public float slashCheckCooldownFloat = 2f;
    private bool isAttacking = false;
    private EnemyAI enemyAI;
    private float attackSpeed = 4f;
    private bool indicatorActive = false;
    private bool attackIndicator = false;
    private GameObject tempAttackIndicator;
    private Vector2 warriorToPlayerAngle;
    private float warriorToPlayerFloat;
    private FormationAnchorBehaviour formationAnchorBehaviour;
    private Rigidbody2D rb;
    private bool isCharging;
    private Vector2 playerPosStartCharge;
    private float chargeSpeed = 10f;
    private Vector2 chargeStartPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackList.Add("slash");
        attackList.Add("regenerate");
        attackList.Add("charge");
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        enemyAI = GetComponentInChildren<EnemyAI>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        formationAnchorBehaviour = enemyAI.assignedSpawnAnchor.GetComponent<FormationAnchorBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {
            if (Vector2.Distance(transform.position, playerMovement.transform.position) < 4 && !slashCheckCooldownBool)
            {
                slashCheckCooldownBool = true;
                StartCoroutine("WarriorSlashCheck");
            }
            if (formationAnchorBehaviour.chargeAttack)
            {
                isAttacking = true;
                currentAttack = "charge";
                StartCoroutine("WarriorIndicatorActivation");
            }
            /*if (is missing health, hasn't been hit in at least 5s))
            {
                isAttacking = true;
                currentAttack = "charge";
                StartCoroutine("WarriorRegenerationCheck");
            }*/
        }
        if (indicatorActive && currentAttack == "slash")
        {
            if (!attackIndicator)
            {

                attackIndicator = true;
                tempAttackIndicator = Instantiate(slashAttackIndicator, transform.position, Quaternion.identity);
            }
            
            tempAttackIndicator.transform.position = transform.position;
            warriorToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y);
            warriorToPlayerFloat = Mathf.Rad2Deg*(Mathf.Atan2(warriorToPlayerAngle.y, warriorToPlayerAngle.x));
            tempAttackIndicator.transform.rotation = Quaternion.Euler(0, 0, warriorToPlayerFloat);

        }
        if (isCharging)
        {
            Debug.Log("CHAAAAAAAAAAARGE");
            Vector2 warriorToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
            rb.MovePosition(warriorToPlayerAngle * chargeSpeed * Time.fixedDeltaTime);
            if (Vector2.Distance(playerPosStartCharge,chargeStartPos) < Vector2.Distance(transform.position, chargeStartPos))
            {
                isCharging = false;
                enemyAI.isCharging = false;
            }
        }
        
    }

    IEnumerator WarriorSlashCheck()
    {
        int chanceToAttack = Random.Range(0, 100);
        if (chanceToAttack <= 99)
        {
            isAttacking = true;
            currentAttack = "slash";
            StartCoroutine("WarriorIndicatorActivation");
            yield break;
        }
        yield return new WaitForSeconds(slashCheckCooldownFloat);
        slashCheckCooldownBool = false;
    }

    IEnumerator WarriorRegenerationCheck()
    {
        
        int chanceToAttack = Random.Range(0, 100);
        if (chanceToAttack <= 20)
        {
            isAttacking = true;
            currentAttack = "regenerate";
            StartCoroutine("WarriorIndicatorActivation");
            yield break;
        }
        yield return new WaitForSeconds(slashCheckCooldownFloat);
        regenerationCheckCooldownBool = false;
    }

    IEnumerator WarriorIndicatorActivation()
    {
        
        if (currentAttack == "slash")
        {
            attackWindupTime = .75f;
            enemyAttackIndicator.SetIndicator(slashIcon, attackWindupTime);
            indicatorActive = true;
        }
        if (currentAttack == "regenerate")
        {
            attackWindupTime = 3f;
            enemyAttackIndicator.SetIndicator(regenerateIcon, attackWindupTime);
            indicatorActive = true;
        }
        if (currentAttack == "charge")
        {
            attackWindupTime = .75f;
            enemyAttackIndicator.SetIndicator(chargeIcon, attackWindupTime);
            indicatorActive = true;
        }

        yield return new WaitForSeconds(attackWindupTime);
        indicatorActive = false;
        attackIndicator = false;
        
        if (currentAttack == "slash") { SlashAttack(); }
        if (currentAttack == "regenerate") { RegenerateAttack(); }
        if (currentAttack == "charge") { ChargeAttack(); }
        GameObject.Destroy(tempAttackIndicator);
        yield return new WaitForSeconds(attackSpeed);
        slashCheckCooldownBool = false;
        isAttacking = false;
    }
    private void SlashAttack()
    {
        Instantiate(slashAttackPrefab, transform.position, Quaternion.Euler(0,0, warriorToPlayerFloat -180));
        indicatorActive = false;
        
    }
    private void RegenerateAttack()
    {
        indicatorActive = false;
       
    }
    private void ChargeAttack()
    {
        indicatorActive = false;
        chargeStartPos = transform.position;
        isCharging = true;
        enemyAI.isCharging = true;
    }
}
