using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OrcBoss : MonoBehaviour
{
    private List<string> attackList = new List<string>();
    private bool isReadyToAttack = true;
    private PlayerMovement playerMovement;
    private EnemyAttackIndicator enemyAttackIndicator;
    private string currentAttack;
    private float attackWindupTime;
    private float attackSpeed = 2f;
    public int spikeStripAmount;
    private Enemy enemy;
    private bool isDead = false;
    private bool spikeTrapIndicatorOver = false;
    public GameObject portalSpawnAura;

    // Attack icons
    public Sprite acidRainIcon;
    public Sprite magicMissileIcon;
    public Sprite spikeStripIcon;
    public Sprite spikeTrapIcon;

    // Prefabs
    public GameObject acidRainPrefab;
    public GameObject magicMissilePrefab;
    public GameObject spikeStripPrefab;
    public GameObject spikeTrapPrefab;

    // Indicators
    public GameObject acidRainIndicator;
    public GameObject magicMissileIndicator;
    public GameObject spikeStripIndicator;
    public GameObject spikeTrapIndicator;
    private GameObject currentIndicator;

    //Animator
    private Animator anim;

    private bool acidRainIndicatorOver;

    void Start()
    {
        enemy = GetComponentInChildren<Enemy>();
        anim = GetComponent<Animator>();
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackList.Add("acidRain");
        attackList.Add("magicMissile");
        attackList.Add("spikeStrip");
        attackList.Add("spikeTrap");
    }

    void Update()
    {
        if (enemy.enemyHealth <= 0 && isDead == false)
        {
            isDead = true;
            OrcBossDeathSequence();
        }
        if (isReadyToAttack && enemy.enemyHealth > 0)
        {
            isReadyToAttack = false;
            StartCoroutine("OrcBossAttackCycle");
        }
        if (currentAttack == "acidRain")
        {
            if (currentIndicator == null)
            {
                currentIndicator = Instantiate(acidRainIndicator, playerMovement.transform.position, Quaternion.identity);
            }
            if (!acidRainIndicatorOver)
            {
                currentIndicator.transform.position = playerMovement.transform.position;
            }
        }
        if (currentAttack == "magicMissile")
        {
            if (currentIndicator == null)
            {
                currentIndicator = Instantiate(magicMissileIndicator, playerMovement.transform.position, Quaternion.identity);
            }
            currentIndicator.transform.position = playerMovement.transform.position;
        }
        if (currentAttack == "spikeStrip")
        {
            
            //no indicator, this is like a rolling attack of spikes that launch themselves towards the player coming from the ground
        }
        if (currentAttack == "spikeTrap")
        {
            if (currentIndicator == null)
            {
                currentIndicator = Instantiate(spikeTrapIndicator, playerMovement.transform.position, Quaternion.identity);
            }

            currentIndicator.transform.position = playerMovement.transform.position;
        }
    }

    IEnumerator OrcBossAttackCycle()
    {
        
        currentAttack = RandomAttack();

        if (currentAttack == "acidRain") { attackWindupTime = .75f; enemyAttackIndicator.SetIndicator(acidRainIcon, attackWindupTime); }
        if (currentAttack == "magicMissile") { attackWindupTime = 0.5f; enemyAttackIndicator.SetIndicator(magicMissileIcon, attackWindupTime); }
        if (currentAttack == "spikeStrip") { attackWindupTime = 1f; enemyAttackIndicator.SetIndicator(spikeStripIcon, attackWindupTime); }
        if (currentAttack == "spikeTrap") { attackWindupTime = 1.25f; enemyAttackIndicator.SetIndicator(spikeTrapIcon, attackWindupTime); }

        yield return new WaitForSeconds(attackWindupTime);

        if (currentAttack == "acidRain") { StartCoroutine("AcidRain"); }
        if (currentAttack == "magicMissile") { StartCoroutine("MagicMissile");  }
        if (currentAttack == "spikeStrip") { StartCoroutine("SpikeStrip"); }
        if (currentAttack == "spikeTrap") { StartCoroutine("SpikeTrap");  }

        yield return new WaitForSeconds(attackSpeed);
        isReadyToAttack = true;
    }

    private string RandomAttack()
    {
        int randomAttack = Random.Range(0, attackList.Count);
        return attackList[randomAttack];
    }

    IEnumerator AcidRain()
    {
        anim.SetInteger("OrcAnimInt", 2);
        Vector2 currentPlayerPos = playerMovement.transform.position;
        acidRainIndicatorOver = true;
        yield return new WaitForSeconds(.25f);
        Instantiate(acidRainPrefab, currentPlayerPos, Quaternion.identity);
        //add animation here
        Destroy(currentIndicator);
        acidRainIndicatorOver = false;
        // AOE circle at player position
        currentAttack = "";
        anim.SetInteger("OrcAnimInt", 0);

    }

    IEnumerator MagicMissile()
    {
        anim.SetInteger("OrcAnimInt", 2);
        Destroy(currentIndicator);
        //kinda realized i cant angle this, but put it in here for cookie points w/u
        Instantiate(magicMissilePrefab, playerMovement.transform.position,Quaternion.identity);
        // Single fast projectile toward player
        currentAttack = "";
        yield return new WaitForSeconds(.05f);
        anim.SetInteger("OrcAnimInt", 0);
    }

    IEnumerator SpikeStrip()
    {
        anim.SetInteger("OrcAnimInt", 3);
        float orcToPlayerDist = Vector2.Distance(transform.position, playerMovement.transform.position);
        Vector2 normOrcToPlayer = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
        Destroy(currentIndicator);
        for (int i = 0; i < (int)orcToPlayerDist+5; i++)
        {
            Instantiate(spikeStripPrefab,(Vector2)transform.position + (normOrcToPlayer*i), Quaternion.identity);
            yield return new WaitForSeconds(.01f);
        }
        currentAttack = "";
        anim.SetInteger("OrcAnimInt", 0);
    }

    IEnumerator SpikeTrap()
    {
        anim.SetInteger("OrcAnimInt", 3);
        Destroy(currentIndicator);
        currentAttack = "";
        //no rotation, just a circular spike trap.
        Vector2 lastPlayerPos = playerMovement.transform.position;
        yield return new WaitForSeconds(.3f);
        Instantiate(spikeTrapPrefab, lastPlayerPos, Quaternion.identity);
        yield return new WaitForSeconds(.05f);
        anim.SetInteger("OrcAnimInt", 0);
    }

    public void OrcBossDeathSequence()
    {
        anim.SetInteger("OrcAnimInt", 4);
        StopAllCoroutines();
        currentAttack = "";
        Destroy(currentIndicator);
    }
    public void DestroyOrcBoss()
    {
        Instantiate(portalSpawnAura, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}