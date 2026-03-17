using System.Collections;
using System.Collections.Generic;
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
    private Animator anim;
    private Enemy enemy;
    private bool isDead = false;

    void Start()
    {
        enemy = GetComponentInChildren<Enemy>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackList.Add("slash");
        attackList.Add("regenerate");
        attackList.Add("charge");
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        enemyAI = GetComponentInChildren<EnemyAI>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (enemy.isDead && !isDead)
        {
            isDead = true;
            DeathSequence();
        }

        if (isAttacking == false && !isDead)
        {
            if (Vector2.Distance(transform.position, playerMovement.transform.position) < 4 && !slashCheckCooldownBool)
            {
                slashCheckCooldownBool = true;
                StartCoroutine("WarriorSlashCheck");
            }
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
            warriorToPlayerFloat = Mathf.Rad2Deg * (Mathf.Atan2(warriorToPlayerAngle.y, warriorToPlayerAngle.x));
            tempAttackIndicator.transform.rotation = Quaternion.Euler(0, 0, warriorToPlayerFloat);
        }

        if (isCharging)
        {
            Vector2 dir = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
            rb.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.fixedDeltaTime);
            if (Vector2.Distance(playerPosStartCharge, chargeStartPos) < Vector2.Distance(transform.position, chargeStartPos))
            {
                isCharging = false;
                enemyAI.isCharging = false;
                formationAnchorBehaviour.chargeAttack = false;
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
            enemyAI.animOverride = true;
            anim.SetInteger("WarriorInt", 2);
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

        if (currentAttack == "slash") { StartCoroutine("SlashAttack"); }
        if (currentAttack == "regenerate") { StartCoroutine("RegenerateAttack"); }
        if (currentAttack == "charge") { StartCoroutine("ChargeAttack"); }
        Destroy(tempAttackIndicator);
        yield return new WaitForSeconds(attackSpeed);
        slashCheckCooldownBool = false;
        isAttacking = false;
    }

    IEnumerator SlashAttack()
    {
        Instantiate(slashAttackPrefab, transform.position, Quaternion.Euler(0, 0, warriorToPlayerFloat - 180));
        indicatorActive = false;
        currentAttack = "";
        enemyAI.animOverride = false;
        yield return null;
    }

    IEnumerator RegenerateAttack()
    {
        indicatorActive = false;
        currentAttack = "";
        yield return null;
    }

    IEnumerator ChargeAttack()
    {
        indicatorActive = false;
        currentAttack = "";
        chargeStartPos = transform.position;
        playerPosStartCharge = playerMovement.transform.position;
        isCharging = true;
        enemyAI.isCharging = true;
        yield return null;
    }

    private void DeathSequence()
    {
        enemyAI.animOverride = true;
        currentAttack = "";
        anim.SetInteger("WarriorInt", 3);
        Destroy(tempAttackIndicator);
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
    }

    public void WarriorDeath()
    {
        Destroy(gameObject);
    }
}