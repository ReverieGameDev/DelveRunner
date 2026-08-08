using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using static FormationAnchorBehaviour;
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
    public float maxEnemyHealth;
    public TextMeshProUGUI hptext;
    public GameObject damageText;
    public bool enfeebled = false;
    public float enfeebleBonusDamage;

    private void Awake()
    {
        emberSystem = FindFirstObjectByType<EmberSystem>();
        if (!enemyData.isREE)
        {
            emberSystem.aliveEnemies++;
        }
        enemyHealth = enemyData.health;
        enemyDamage = enemyData.damage;
        enemySpeed = enemyData.speed;
        enemyHealth *= Mathf.Pow(1.08f, emberSystem.waveNumber - 1);
        maxEnemyHealth = enemyHealth;
    }

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
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
    public void EnemyFrontlineHealth(int damageDealt)
    {
        enemyAI.assignedSpawnAnchorScript.frontlineCurrentHP -= damageDealt;
        enemyAI.assignedSpawnAnchorScript.EvaluateFormationState(FormationCheck.LowFrontline);
    }

    public void HealEnemy(float damageHealed)
    {
         enemyHealth = Mathf.Min(enemyHealth+damageHealed, maxEnemyHealth);
         hptext.text = (int)enemyHealth + " / " + (int)maxEnemyHealth;
         if (hpBar != null) hpBar.value = enemyHealth / maxEnemyHealth;
    }
    public void reduceHp(float damageTaken, int hitCount = 1, bool isCrit = false)
    {
        Debug.Log(enfeebled + " / " + enfeebleBonusDamage);
        if (enemyHealth <= 0) return;
        if (playerCombat.curtainCallActive && !enemyData.isBoss && isCrit && enemyHealth/maxEnemyHealth <= playerCombat.curtainCallExecute)
        {
            damageTaken = 9999f;
        }
        int damageTakenInt;
        if (enfeebled)
        {
            damageTakenInt = (int)Mathf.Round((damageTaken) * enfeebleBonusDamage);
        }
        else
        {
            damageTakenInt = (int)Mathf.Round(damageTaken);
        }
        int damageTakenTotal = (int)Mathf.Round((damageTakenInt) * hitCount);
        float damageDealt = Mathf.Min(damageTakenInt, enemyHealth);

        enemyHealth = Mathf.Clamp(enemyHealth - damageTakenTotal, 0, maxEnemyHealth);
        if (enemyAI.isFrontline)
        {
            EnemyFrontlineHealth((int)damageDealt);
        }
        //if (enemyAI.isBackline)

        hptext.text = (int)enemyHealth + " / " + (int)maxEnemyHealth;
        if (enemyHealth <= 0)
        {
            playerCombat.OnEnemyKilled();
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
        if (hitCount > 1) { StartCoroutine(MultipleDamageHits(damageTakenInt, hitCount, isCrit)); }
        else 
        {
            
            GameObject popup = Instantiate(damageText, transform.position, Quaternion.identity);
            popup.GetComponent<EnemyDamageNumbers>().DamageNumberSetup(damageTakenInt, isCrit);
            popup.transform.SetAsLastSibling();
        }
        StartCoroutine(FlashOnDamage());
    }
    IEnumerator FlashOnDamage()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(.075f);
            sprite.color = Color.white;
        }
    }
    IEnumerator MultipleDamageHits(int damageTaken, int hitCount, bool isCrit)
    {

        for (int i = 0; i < hitCount; i++)
        {
            GameObject popup = Instantiate(damageText, transform.position, Quaternion.identity);
            Canvas c = popup.GetComponentInChildren<Canvas>();
            c.sortingOrder += i;
            popup.GetComponent<EnemyDamageNumbers>().DamageNumberSetup(damageTaken, isCrit);
            yield return new WaitForSeconds(.25f);
        }
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