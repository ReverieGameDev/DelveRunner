using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonSummoner : MonoBehaviour
{
    List<string> attackList = new List<string>();
    public bool isReadyTofire = true;
    public GameObject homingProjectile;
    public GameObject aoeCircleIndicator;
    public GameObject aoeCircleAttack;
    public GameObject catSummon;
    private PlayerMovement playerMovement;
    private EnemyAttackIndicator enemyAttackIndicator;
    public Sprite homingAttackIcon;
    public Sprite aoeCircleIcon;
    public Sprite summonIcon;
    public float attackWindupTime;
    private float attackSpeed = 1f;
    private bool isIndicatorActive = false;
    private string currentAttack;
    public float indicatorLength = 20f;
    private int summonCount = 2;
    public List<GameObject> summonList = new List<GameObject>();
    public int aoeCircleAmount = 5;
    private GameObject aoeCircleIndicatorTBD;
    public Vector2 summoningLocation;
    private bool hasCircleSpawned = false;
    private MapGenerator mapGenerator;
    private GameObject spawnedProjectile;
    private EnemyAI enemyAI;
    private Animator anim;
    private Enemy enemy;
    private bool isDead = false;

    void Start()
    {
        enemy = GetComponentInChildren<Enemy>();
        anim = GetComponentInChildren<Animator>();
        enemyAI = GetComponentInChildren<EnemyAI>();
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackList.Add("homingAttack");
        attackList.Add("aoeCircleAttack");
        attackList.Add("summonAttack");
        attackList.Add("skipAttack");
    }

    void FixedUpdate()
    {
        if (enemy.isDead && !isDead)
        {
            isDead = true;
            DeathSequence();
        }

        if (isReadyTofire && !isDead)
        {
            isReadyTofire = false;
            StartCoroutine("SkeletonSummonerAttackCycle");
        }
        if (isIndicatorActive && currentAttack == "aoeCircleAttack")
        {
            if (!hasCircleSpawned)
            {
                aoeCircleIndicatorTBD = Instantiate(aoeCircleIndicator, transform.position, Quaternion.identity);
                hasCircleSpawned = true;
            }
            aoeCircleIndicatorTBD.transform.position = transform.position;
        }
    }

    public IEnumerator SkeletonSummonerAttackCycle()
    {
        currentAttack = RandomAttack();
        if (currentAttack == "homingAttack")
        {
            enemyAI.animOverride = true;
            anim.SetInteger("NecromancerInt", 2);
            isIndicatorActive = true;
            attackWindupTime = .5f;
            enemyAttackIndicator.SetIndicator(homingAttackIcon, attackWindupTime);
        }
        if (currentAttack == "aoeCircleAttack")
        {
            enemyAI.animOverride = true;
            anim.SetInteger("NecromancerInt", 2);
            isIndicatorActive = true;
            attackWindupTime = 2.5f;
            enemyAttackIndicator.SetIndicator(aoeCircleIcon, attackWindupTime);
        }
        if (currentAttack == "summonAttack")
        {
            enemyAI.animOverride = true;
            anim.SetInteger("NecromancerInt", 2);
            isIndicatorActive = true;
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(summonIcon, attackWindupTime);
        }
        if (currentAttack == "skipAttack")
        {
            StartCoroutine("SkipAttack");
            yield break;
        }
        yield return new WaitForSeconds(attackWindupTime);
        if (currentAttack == "homingAttack") { StartCoroutine("HomingAttack"); }
        if (currentAttack == "aoeCircleAttack") { StartCoroutine("AoeCircleAttack"); }
        if (currentAttack == "summonAttack") { StartCoroutine("SummonAttack"); }

        yield return new WaitForSeconds(attackSpeed);
        isReadyTofire = true;
    }

    public string RandomAttack()
    {
        int attackChance = Random.Range(0, 2);
        if (attackChance == 0)
        {
            int randomAttack = Random.Range(0, 3);
            return attackList[randomAttack];
        }
        else
        {
            return attackList[3];
        }
    }

    IEnumerator SkipAttack()
    {
        yield return new WaitForSeconds(3f);
        isReadyTofire = true;
    }

    IEnumerator HomingAttack()
    {
        isIndicatorActive = false;
        currentAttack = "";
        spawnedProjectile = Instantiate(homingProjectile, transform.position, Quaternion.identity);
        enemyAI.animOverride = false;
        yield return null;
    }

    IEnumerator AoeCircleAttack()
    {
        isIndicatorActive = false;
        currentAttack = "";
        hasCircleSpawned = false;
        Destroy(aoeCircleIndicatorTBD);
        Instantiate(aoeCircleAttack, transform.position, Quaternion.identity);
        enemyAI.animOverride = false;
        yield return null;
    }

    IEnumerator SummonAttack()
    {
        int maxAttempts = 100;
        int attempts = 0;
        isIndicatorActive = false;
        currentAttack = "";
        for (int i = 0; i < summonCount; i++)
        {
            attempts++;
            if (attempts > maxAttempts) break;

            Vector2 randomSummonSpace = new Vector2(Random.Range(transform.position.x + 10, transform.position.x + 30), Random.Range(transform.position.y + 10, transform.position.y + 30));
            if (mapGenerator.mapArray[(int)randomSummonSpace.x, (int)randomSummonSpace.y] == 1)
            {
                Instantiate(catSummon, randomSummonSpace, Quaternion.identity);
            }
            else
            {
                i--;
            }
        }
        enemyAI.animOverride = false;
        yield return null;
    }

    private void DeathSequence()
    {
        enemyAI.animOverride = true;
        currentAttack = "";
        isReadyTofire = false;
        anim.SetInteger("NecromancerInt", 3);
        Destroy(aoeCircleIndicatorTBD);
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
    }

    public void NecromancerDeath()
    {
        Destroy(gameObject);
    }
}