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
    [SerializeField] private DropOnDeath dropOnDeath;
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
    public void reduceHp(float damageTaken, int hitCount = 1, bool isCrit = false, WeaponStatusEffect type = WeaponStatusEffect.None)
    {
        if (enemyHealth <= 0 ) return;
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
        if (enemyAI != null && enemyAI.isFrontline)
        {
            EnemyFrontlineHealth((int)damageDealt);
        }
        //if (enemyAI.isBackline)

        hptext.text = (int)enemyHealth + " / " + (int)maxEnemyHealth;
        if (enemyHealth <= 0)
        {
            playerCombat.OnEnemyKilled(this);
            DropOnDeath drop = GetComponent<DropOnDeath>();
            drop.DropItems(transform.position);
            
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
            }
            else if (enemyData.isREE)
            {
                isDead = true;
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
            }
        }
        if (hpBar != null) hpBar.value = enemyHealth / maxEnemyHealth;
        if (hitCount > 1) { StartCoroutine(MultipleDamageHits(damageTakenInt, hitCount, isCrit)); }
        else 
        {
            
            GameObject popup = Instantiate(damageText, transform.position, Quaternion.identity);
            popup.GetComponent<EnemyDamageNumbers>().DamageNumberSetup(damageTakenInt, isCrit,type,enfeebled);
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
            popup.GetComponent<EnemyDamageNumbers>().DamageNumberSetup(damageTaken, isCrit, WeaponStatusEffect.None, enfeebled);
            yield return new WaitForSeconds(.25f);
        }
    }

}