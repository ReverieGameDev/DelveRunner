using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject xpDrop;
    private float enemyHealth;
    private float enemyDamage;
    public float enemySpeed;
    private Slider hpBar;
    private WaveManager waveManager;
    public GameObject money1;
    private PlayerCombat playerCombat;
    private SpriteRenderer spriteRenderer;
    private string enemyName;
    public bool inExplosionRange;
    public bool bossMonster = false;
    private DeathBossScript deathBossScript;
    private bool isDead;
    public bool isMinion = false;

    
    

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        waveManager = FindFirstObjectByType<WaveManager>();
        deathBossScript = FindFirstObjectByType<DeathBossScript>();
        enemyHealth = enemyData.health;
        enemyDamage = enemyData.damage;
        enemyName = enemyData.mobName;
        enemySpeed = enemyData.speed;

        hpBar = GetComponentInChildren<Slider>();
        hpBar.value = 1f;
        if (enemyName == "DeathSummon")
        {
            isMinion = true;
        }
    }

    private void Update()
    {
        float playerX = playerCombat.transform.position.x;
        if (playerX < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        
    }

    public void reduceHp(float damageTaken)
    {
        if (bossMonster && isDead) return;
        enemyHealth -= damageTaken;

        if (enemyHealth <= 0 && bossMonster == false && isMinion == false)
        {
            Instantiate(xpDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
            GoldRandomizer();
        }
        else if (enemyHealth <= 0 && bossMonster == true && enemyName == "DeathBoss")
        {
            isDead = true;
            Debug.Log("death boss should die here");
            deathBossScript.StopAllCoroutines();
            deathBossScript.StartCoroutine("DeathBossDeath");
        }
        else if (enemyHealth <= 0 && isMinion == true)
        {
            Destroy(gameObject);
        }

            hpBar.value = enemyHealth / enemyData.health;
    }

    private void GoldRandomizer()
    {
        int goldRandomizer = (Random.Range(0, 101));
        if (goldRandomizer < 10)
        {
            Instantiate(money1, new Vector3(transform.position.x+3, transform.position.y), Quaternion.identity);
        }
    }


    IEnumerator MushroomExplode()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.25f);
        }

        if (inExplosionRange)
        {
            playerCombat.DamagePlayer();
        }
        GetComponent<Animator>().SetTrigger("Explode");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}


