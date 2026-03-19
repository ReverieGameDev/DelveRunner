using System.Collections;
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


    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
        enemyHealth = enemyData.health;
        enemyDamage = enemyData.damage;
        enemySpeed = enemyData.speed;
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
    public void reduceHp(float damageTaken)
    {
        if (enemyHealth <= 0) return;
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        enemyHealth -= damageTakenInt;
        if (enemyHealth <= 0)
        {
            if (enemyAI != null)
            {
                isDead = true;
                enemyAI.currentState = EnemyState.Death;
                StartCoroutine("GoldAndExpRandomizer");
            }
            else if (enemyData.isREE)
            {
                isDead = true;
                StartCoroutine("GoldAndExpRandomizer");
            }
            else
            {
                // Boss path — drop gold instantly, no coroutine
                GoldRandomizerBoss();
            }
        }
        if (hpBar != null) hpBar.value = enemyHealth / enemyData.health;
    }
    IEnumerator GoldAndExpRandomizer()
    {
        int goldChance = Random.Range(0, 101);
        int xpRandomizer = Random.Range(0, 4);
        int goldRandomizer = Random.Range(0, 6);
        if (goldChance < 40)
        {
            for (int i = 0; i < goldRandomizer; i++)
            {
                int randomX = Random.Range(-5, 4);
                int randomY = Random.Range(-5, 4);
                Instantiate(money1, new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
                
            }
        }

        for(int i =0; i < xpRandomizer; i++)
        {
            int randomX = Random.Range(-5, 4);
            int randomY = Random.Range(-5, 4);
            Instantiate(xpDrop, new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
           
        }
        
        return null;
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