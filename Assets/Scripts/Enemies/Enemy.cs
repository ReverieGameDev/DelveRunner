using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject xpDrop;
    public float enemyHealth;
    private float enemyDamage;
    public float enemySpeed;
    private Slider hpBar;
    public GameObject money1;
    private PlayerCombat playerCombat;
    private SpriteRenderer spriteRenderer;
    private EnemyAI enemyAI;
    public bool isDead = false;
    public GameObject emberPickup;
    private EmberSystem emberSystem;
    private float maxEnemyHealth;
    public TextMeshProUGUI hptext;
    public GameObject damageText;


    void Start()
    {
        
        emberSystem = FindFirstObjectByType<EmberSystem>();
        if (!enemyData.isREE)
        {
            emberSystem.aliveEnemies++;
        }
        
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
        enemyHealth = enemyData.health;
        enemyDamage = enemyData.damage;
        enemySpeed = enemyData.speed;
        enemyHealth *= Mathf.Pow(1.08f, emberSystem.waveNumber - 1);
        maxEnemyHealth = enemyHealth;
        hpBar = GetComponentInChildren<Slider>();
        if (hpBar != null) hpBar.value = 1f;
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
    public void reduceHp(float damageTaken, bool isCrit = false)
    {

        if (enemyHealth <= 0) return;
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        enemyHealth -= damageTakenInt;
        hptext.text = (int)enemyHealth + " / " + (int)maxEnemyHealth;
        if (enemyHealth <= 0)
        {
            if (enemyAI != null)
            {
                emberSystem.aliveEnemies--;
                if (emberSystem.aliveEnemies == 0)
                {
                    emberSystem.NewWave();
                }
                isDead = true;
                enemyAI.currentState = EnemyState.Death;
                if (enemyAI.isBackline) { enemyAI.ReduceFromBackline(); }
                StartCoroutine("GoldAndExpRandomizer");
                StartCoroutine("DropEmber");
            }
            else if (enemyData.isREE)
            {
                isDead = true;
                StartCoroutine("GoldAndExpRandomizer");
                StartCoroutine("DropEmber");
            }
            else if (!enemyData.isBoss)
            {
                emberSystem.aliveEnemies--;
                if (emberSystem.aliveEnemies == 0)
                {
                    emberSystem.NewWave();
                }
                isDead = true;
                Destroy(gameObject);
            }
            else
            {
                emberSystem.aliveEnemies--;
                GoldRandomizerBoss();
            }
        }
        if (hpBar != null) hpBar.value = enemyHealth / maxEnemyHealth;
        GameObject popup = Instantiate(damageText, transform.position, Quaternion.identity);
        popup.GetComponent<EnemyDamageNumbers>().DamageNumberSetup(damageTakenInt, isCrit);
    }
    IEnumerator DropEmber()
    {
        if (emberPickup == null) yield break;
        int emberChance = Random.Range(0, 101);
        if (emberChance < 20)
        {
            Instantiate(emberPickup, transform.position, Quaternion.identity);
        }
        yield break;
    }

    IEnumerator GoldAndExpRandomizer()
    {
        int goldChance = Random.Range(0, 101);
        int xpRandomizer = Random.Range(1, 5);
        int goldRandomizer = Random.Range(0, 6);
        if (goldChance < 40 && money1 != null)
        {
            for (int i = 0; i < goldRandomizer; i++)
            {
                int randomX = Random.Range(-5, 4);
                int randomY = Random.Range(-5, 4);
                Instantiate(money1, new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
            }
        }
        if (xpDrop != null)
        {
            for (int i = 0; i < xpRandomizer; i++)
            {
                int randomX = Random.Range(-5, 4);
                int randomY = Random.Range(-5, 4);
                Instantiate(xpDrop, new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
            }
        }
        yield break;
    }
    public void GoldRandomizerBoss()
    {
        int goldRandomizer = Random.Range(0, 6);
        for (int i = 0; i < goldRandomizer; i++)
        {
            int randomX = Random.Range(-5, 4);
            int randomY = Random.Range(-5, 4);
            Instantiate(money1, new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
        }
    }
}