using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class SkeletonArcher : MonoBehaviour
{
    List<string> attackList = new List<string>();
    public bool isReadyTofire = true;
    public bool isREE = false;
    public GameObject arrow;
    private PlayerMovement playerMovement;
    private EnemyAttackIndicator enemyAttackIndicator;
    public Sprite arrowShotIcon;
    public Sprite multiShotIcon;
    public Sprite arrowVolleyIcon;
    public Sprite arrowRainIcon;
    public GameObject arrowTrajectory;
    public float attackWindupTime;
    private float attackSpeed = 3f;
    private bool isIndicatorActive = false;
    private string currentAttack;
    private Vector2 archerToPlayerAngle;
    public float indicatorLength = 20f;
    private int multiShotCount = 0;
    public List<GameObject> arrowVolleyList = new List<GameObject>();
    private List<GameObject> volleyIndicators = new List<GameObject>();
    public int arrowVolleyAmount = 5;
    private GameObject currentIndicator;
    private bool indicatorShown = false;
    private Animator anim;
    private EnemyAI enemyAI;
    private Enemy enemy;
    private bool isDead = false;

    void Start()
    {
        enemy = GetComponentInChildren<Enemy>();
        anim = GetComponentInChildren<Animator>();
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        if (!isREE) { enemyAI = GetComponentInChildren<EnemyAI>(); enemyAI.isBackline = true; }
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackList.Add("arrowShot");
        attackList.Add("arrowVolley");
        attackList.Add("multiShot");
        attackList.Add("arrowRain");
    }

    void FixedUpdate()
    {
        if (isREE && Vector2.Distance(transform.position, playerMovement.transform.position) > 40f)
        {
            currentAttack = "";
            isReadyTofire = false;
            anim.speed = 1f;
            anim.SetInteger("ArcherInt", 3);
            arrowVolleyList.Clear();
            GetComponent<Collider2D>().enabled = false;
            StopAllCoroutines();
            Destroy(gameObject);
        }
        if (enemy.isDead && !isDead)
        {
            anim.speed = 1f;
            enemy.isDead = true;
            isDead = true;
            DeathSequence();
        }
        if (isReadyTofire)
        {
            isReadyTofire = false;
            StartCoroutine("SkeletonArcherAttackCycle");
        }
    }

    public IEnumerator SkeletonArcherAttackCycle()
    {
        currentAttack = RandomSkeletonArcherAttack();
        if (currentAttack == "arrowShot")
        {
            isIndicatorActive = true;
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(arrowShotIcon, attackWindupTime);
            if (!isREE) enemyAI.animOverride = true;
            anim.SetInteger("ArcherInt", 2);
            anim.speed = 2.5f;
        }
        if (currentAttack == "arrowVolley")
        {
            isIndicatorActive = true;
            attackWindupTime = 2.5f;
            enemyAttackIndicator.SetIndicator(arrowVolleyIcon, attackWindupTime);
            if (!isREE) enemyAI.animOverride = true;
            anim.SetInteger("ArcherInt", 2);
        }
        if (currentAttack == "multiShot")
        {
            isIndicatorActive = true;
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(multiShotIcon, attackWindupTime);
            if (!isREE) enemyAI.animOverride = true;
            anim.SetInteger("ArcherInt", 2);
            anim.speed = 2.5f;
        }
        if (currentAttack == "arrowRain") { attackWindupTime = .75f; enemyAttackIndicator.SetIndicator(arrowRainIcon, attackWindupTime); }
        if (!isREE) enemyAI.isAttacking = true;
        yield return new WaitForSeconds(attackWindupTime);
        if (!isREE) enemyAI.isAttacking = false;
        if (currentAttack == "arrowShot") { StartCoroutine("ArrowShot"); }
        if (currentAttack == "arrowVolley") { StartCoroutine("ArrowVolley"); }
        if (currentAttack == "multiShot") { StartCoroutine("MultiShot"); }
        if (currentAttack == "arrowRain") { StartCoroutine("ArrowRain"); }
        yield return new WaitForSeconds(attackSpeed);
        isReadyTofire = true;
    }

    public string RandomSkeletonArcherAttack()
    {
        int randomAttack = UnityEngine.Random.Range(0, attackList.Count - 1);
        string currentAttack;
        return currentAttack = attackList[randomAttack];
    }

    IEnumerator ArrowShot()
    {
        ResumeShot();
        currentAttack = "";
        indicatorShown = false;
        isIndicatorActive = false;
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot", 0);
        if (!isREE) enemyAI.animOverride = false;
        anim.speed = 1f;
        return null;
    }
    IEnumerator ArrowVolley()
    {
        currentAttack = "";
        ResumeShot();
        indicatorShown = false;
        arrowVolleyList.Clear();
        isIndicatorActive = false;
        for (int i = 0; i < arrowVolleyAmount; i++)
        {
            int offset = 30 - (15 * i);
            GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
            spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowVolley", offset);
            arrowVolleyList.Add(spawnedArrow);
        }
        if (!isREE) enemyAI.animOverride = false;
        anim.speed = 1f;
        return null;
    }
    IEnumerator MultiShot()
    {
        ResumeShot();
        currentAttack = "";
        indicatorShown = false;
        isIndicatorActive = false;
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot", 0);
        multiShotCount++;
        StartCoroutine("MultiShotDelay");
        if (!isREE) enemyAI.animOverride = false;
        anim.speed = 1f;
        return null;
    }
    IEnumerator ArrowRain()
    {
        return null;
    }

    IEnumerator MultiShotDelay()
    {
        multiShotCount++;
        yield return new WaitForSeconds(.3f);
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot", 0);
        if (multiShotCount < 3)
        {
            StartCoroutine("MultiShotDelay");
        }
        else if (multiShotCount >= 3)
        {
            isIndicatorActive = false;
            multiShotCount = 0;
            StopCoroutine("MultiShotDelay");
        }
    }
    public void DeathSequence()
    {
        currentAttack = "";
        if (!isREE) enemyAI.animOverride = true;
        isReadyTofire = false;
        anim.speed = 1f;
        anim.SetInteger("ArcherInt", 3);
        arrowVolleyList.Clear();
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
    }
    public void DestroyArcher()
    {
        Destroy(gameObject);
    }
    public void PauseKnock()
    {
        if (!enemy.isDead)
        {
            anim.speed = 0f;
        }
    }
    public void ResumeShot()
    {
        if (!enemy.isDead)
        {
            anim.speed = 1f;
        }
    }
}