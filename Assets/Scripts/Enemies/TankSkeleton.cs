using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
    public GameObject warriorLeapIndicator;
    public Sprite leapIcon;
    public float leapCheckCooldownFloat = 3f;
    private bool leapCheckCooldownBool = false;
    private bool isCasting = false;
    public GameObject warriorShadow;
    public GameObject leapIndicator;
    private bool leapFollowIndicator = false;
    private GameObject currentLeapIndicator;
    public GameObject leapDust;
    public float leapDamage = 15f;
    public bool canLeapDamage = false;
    public GameObject leapHitboxPrefab;
    private PlayerStatusEffects playerStatusEffects;


    void Start()
    {
        playerStatusEffects = FindFirstObjectByType<PlayerStatusEffects>();
        enemy = GetComponentInChildren<Enemy>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackList.Add("slash");
        attackList.Add("regenerate");
        attackList.Add("charge");
        attackList.Add("leap");
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
        if (isDead) return;
        if (!enemyAI.isAttacking && !isDead && !slashCheckCooldownBool)
        {
            if (Vector2.Distance(transform.position, playerMovement.transform.position) < 4 && !slashCheckCooldownBool)
            {
                slashCheckCooldownBool = true;
                StartCoroutine("WarriorSlashCheck");
            }
        }

        if (enemyAI.assignedSpawnAnchor != null &&
    enemyAI.assignedSpawnAnchor.GetComponent<FormationAnchorBehaviour>().canWarriorLeap &&
    !enemyAI.isAttacking && !leapCheckCooldownBool)
        {
            leapCheckCooldownBool = true;
            StartCoroutine("WarriorLeapCheck");
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
            tempAttackIndicator.transform.rotation = Quaternion.Euler(0, 0, warriorToPlayerFloat -180);
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

        if (leapFollowIndicator)
        {
            currentLeapIndicator.GetComponent<Rigidbody2D>().MovePosition(new Vector2(playerMovement.transform.position.x,playerMovement.transform.position.y-.5f));
        }
    }

    IEnumerator WarriorLeapCheck()
    {
        int chanceToAttack = Random.Range(0, 100);
        if (chanceToAttack <= 25)
        {
            enemyAI.isAttacking = true;
            currentAttack = "leap";
            StartCoroutine("WarriorIndicatorActivation");
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(leapCheckCooldownFloat);
            leapCheckCooldownBool = false;
        }
    }

    IEnumerator WarriorSlashCheck()
    {
        int chanceToAttack = Random.Range(0, 100);
        if (chanceToAttack <= 30)
        {
            enemyAI.isAttacking = true;
            currentAttack = "slash";
            StartCoroutine("WarriorIndicatorActivation");
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(slashCheckCooldownFloat);
            slashCheckCooldownBool = false;
        }
            
    }

    IEnumerator WarriorRegenerationCheck()
    {
        int chanceToAttack = Random.Range(0, 100);
        if (chanceToAttack <= 20)
        {
            enemyAI.isAttacking = true;
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
        if (currentAttack == "leap")
        {
            anim.SetInteger("WarriorInt", 4);
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(leapIcon, attackWindupTime);
            leapFollowIndicator = true;
            currentLeapIndicator = Instantiate(leapIndicator, playerMovement.transform.position,Quaternion.identity);
            indicatorActive = true;
        }

        yield return new WaitForSeconds(attackWindupTime);
        indicatorActive = false;
        attackIndicator = false;

        if (currentAttack == "slash") { StartCoroutine("SlashAttack"); }
        if (currentAttack == "leap") { StartCoroutine("WarriorLeap"); }
        if (currentAttack == "regenerate") { StartCoroutine("RegenerateAttack"); }
        if (currentAttack == "charge") { StartCoroutine("ChargeAttack"); }
        Destroy(tempAttackIndicator);
        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator SlashAttack()
    {
        Vector2 offsetDir = warriorToPlayerAngle.normalized;
        float forwardOffset = -2f; // tune this
        Vector3 spawnPos = transform.position + (Vector3)(offsetDir * forwardOffset);

        Instantiate(slashAttackPrefab, spawnPos, Quaternion.Euler(0, 0, warriorToPlayerFloat - 90));
        indicatorActive = false;
        currentAttack = "";
        enemyAI.animOverride = false;
        enemyAI.isAttacking = false;
        slashCheckCooldownBool = false;
        yield return null;
    }
    IEnumerator WarriorLeap()
    {
        leapFollowIndicator = false;
        UnpauseWarriorLeap();
        Vector2 jumpStart = transform.position;
        Vector2 targetPos = playerMovement.transform.position;
        Vector2 direction = (targetPos - jumpStart).normalized;
        float totalDistance = Vector2.Distance(jumpStart, targetPos);
        float leapSpeed = 15f;
        leapSpeed = (leapSpeed * (totalDistance / 7f));
        
        while (Vector2.Distance(transform.position, jumpStart) < totalDistance)
        {
            rb.MovePosition((Vector2)transform.position + direction * leapSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
            if (Vector2.Distance(transform.position, jumpStart) < totalDistance / 2)
            {
                transform.localScale += new Vector3(0.01f, 0.01f, 1f);
                warriorShadow.transform.localScale += new Vector3(0.01f, 0.01f, 1f);
            }
            if (Vector2.Distance(transform.position, jumpStart) >= totalDistance / 2)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 1f);
                warriorShadow.transform.localScale -= new Vector3(0.01f, 0.01f, 1f);
            }
        }
        Destroy(currentLeapIndicator);
        transform.localScale = new Vector3(1f, 1f, 1f);
        warriorShadow.transform.localScale = new Vector3(2.1f, 1.3f, 1f);
        leapCheckCooldownBool = false;
        anim.SetInteger("WarriorInt", 0);
        anim.speed = 1;
        enemyAI.isAttacking = false;
        Instantiate(leapDust, targetPos, Quaternion.identity);
        GameObject hitbox = Instantiate(leapHitboxPrefab, new Vector2(transform.position.x, transform.position.y - .5f), Quaternion.identity);
        hitbox.GetComponent<WarriorLeap>().owner = this;
    }

    public void PauseWarriorLeap()
    {
        anim.speed = 0;
    }
    public void UnpauseWarriorLeap()
    {
        anim.speed = 1;
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
        anim.speed = 1;
        leapFollowIndicator = false;
        if (currentLeapIndicator != null) { Destroy(currentLeapIndicator); }
        enemyAI.animOverride = true;
        currentAttack = "";
        anim.SetInteger("WarriorInt", 3);
        Destroy(tempAttackIndicator);
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
    }

    public void ResetAnimToIdle()
    {
        anim.SetInteger("WarriorInt", 0);
    }

    public void WarriorDeath()
    {
        Destroy(gameObject);
    }
}