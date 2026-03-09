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
    

    void Start()
    {
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
        if (isReadyTofire)
        {
            isReadyTofire = false;
            StartCoroutine("SkeletonSummonerAttackCycle");
        }
        if (isIndicatorActive && currentAttack == "homingAttack")
        {
            //no indicator as this will simply follow the player.
        }
        if (isIndicatorActive && currentAttack == "aoeCircleAttack")
        {
            if (!hasCircleSpawned)
            {
                aoeCircleIndicatorTBD = Instantiate(aoeCircleIndicator, transform.position, Quaternion.identity);
                hasCircleSpawned = true;
            }
            aoeCircleIndicatorTBD.transform.position = transform.position;
            //no math necessary here, it will be donut shaped with the summoner/formation in the center
        }
        if (isIndicatorActive && currentAttack == "summonAttack")
        {
            //need math done for where summon will be here (cant spawn oob)
        }
    }

    public IEnumerator SkeletonSummonerAttackCycle()
    {
        currentAttack = RandomAttack();
        if (currentAttack == "homingAttack")
        {
            isIndicatorActive = true;
            attackWindupTime = .5f;
            enemyAttackIndicator.SetIndicator(homingAttackIcon, attackWindupTime);
        }
        if (currentAttack == "aoeCircleAttack")
        {
            isIndicatorActive = true;
            attackWindupTime = 2.5f;
            enemyAttackIndicator.SetIndicator(aoeCircleIcon, attackWindupTime);
        }
        if (currentAttack == "summonAttack")
        {
            isIndicatorActive = true;
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(summonIcon, attackWindupTime);
        }
        if (currentAttack == "skipAttack")// added new attack, basically a null attack, this will allow me to scale difficulty
        {
            StartCoroutine("SkipAttack");
            StopCoroutine("SkeletonSummonerAttackCycle");
        }
        yield return new WaitForSeconds(attackWindupTime);
        if (currentAttack == "homingAttack") { HomingAttack(); }
        if (currentAttack == "aoeCircleAttack") { AoeCircleAttack(); }
        if (currentAttack == "summonAttack") { SummonAttack(); }
        
        yield return new WaitForSeconds(attackSpeed);
        isReadyTofire = true;
    }

    public string RandomAttack()
    {
        int randomAttack = Random.Range(0, attackList.Count);
        string currentAttack;
        return currentAttack = attackList[randomAttack];
    }
    IEnumerator SkipAttack()
    {
        yield return new WaitForSeconds(3f);
        isReadyTofire = true;
    }
    private void HomingAttack()
    {
        isIndicatorActive = false;
        spawnedProjectile = Instantiate(homingProjectile, transform.position, Quaternion.identity);
        Debug.Log(spawnedProjectile);
    }
    private void AoeCircleAttack()
    {
        isIndicatorActive = false;
        hasCircleSpawned = false;//turn this off here to have it ready for the next round
        Destroy(aoeCircleIndicatorTBD);
        Instantiate(aoeCircleAttack, transform.position, Quaternion.identity);
    }
    private void SummonAttack()
    {
        isIndicatorActive = false;
        for(int i = 0; i< summonCount; i++)
        {
            Vector2 randomSummonSpace = new Vector2(Random.Range(transform.position.x + 10, transform.position.x + 30), Random.Range(transform.position.y + 10, transform.position.y + 30));
            if (mapGenerator.mapArray[(int)randomSummonSpace.x,(int)randomSummonSpace.y] == 1)
            {
                Instantiate(catSummon, randomSummonSpace, Quaternion.identity);
            }
            else
            {
                i--;
            }
        }
    }
}