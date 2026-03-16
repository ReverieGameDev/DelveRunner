using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;


    // References
    public PlayerData playerData;
    private PlayerMovement playerMovement;
    private AugmentManager augmentManager;
    public AttackManager attackManager;

    // Combat
    public GameObject closestCurrentEnemy;

    //Money
    public int playerMoney = 0;

    // Stats
    private int currentPlayerHealth;
    public float attack = 1f;
    public float attackSpeed = 1f;
    public int critChance = 5;
    public float critDamage = 1.5f;
    public float armor = 0f;
    public float maxHealth = 100f;
    public float dodge = 0f;
    public float movementSpeed = 5f;
    public float xpGain = 1f;
    public float goldGain = 1f;

    // UI
    public Slider playerHpBar;
    public Slider playerXpBar;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI moneyText;

    // State
    public bool iFrames = false;
    public int playerXp;
    public int playerLevel = 1;
    public int delveLevel = 0;
    public int augmentsOwed = 0;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        augmentManager = FindFirstObjectByType<AugmentManager>();
        attackManager = FindFirstObjectByType<AttackManager>();

        currentPlayerHealth = (int)playerData.playerHp;
        playerHpBar.value = 1.0f;
    }

    void Update()
    {
        FindClosestEnemy();
    }
    public int CalcWeaponDamage(float damage)
    {
        int critRoll = Random.Range(0, 101);
        int processedDamage = 0;

        if (critRoll < critChance)
        {
            processedDamage = (int)(Mathf.Round(damage * attack * critDamage));
        }
        else
        {
            processedDamage = (int)(Mathf.Round(damage * attack));
        }
        return processedDamage;
    }
    private void FindClosestEnemy()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Vector2 currentEnemy = enemy.transform.position;
            if (closestCurrentEnemy == null)
            {
                closestCurrentEnemy = enemy;
            }
            else if ((playerMovement.playerPosition - currentEnemy).magnitude < (playerMovement.playerPosition - (Vector2)closestCurrentEnemy.transform.position).magnitude)
            {
                closestCurrentEnemy = enemy;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Money1"))
        {
            playerMoney++;
            moneyText.text = ": " + playerMoney;
        }
    }

    public void DamagePlayer(float damageTaken)
    {
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        if (iFrames) return;
        StartCoroutine("IFrames");
        currentPlayerHealth -= damageTakenInt;
        playerHpBar.value = currentPlayerHealth / playerData.playerHp;

        if (currentPlayerHealth <= 0)
        {
            GameOver();
        }
    }

    IEnumerator IFrames()
    {
        iFrames = true;
        StartCoroutine("IFrameAnimation");
        yield return new WaitForSeconds(.66f);
        iFrames = false;
    }

    IEnumerator IFrameAnimation()
    {
        for (int i = 0; i < 9; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.06f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.06f);
        }
    }

    public void addExp()
    {
        playerXp += 1;
        playerXpBar.value = playerXp / 100f;

        while (playerXp >= 100)
        {
            playerLevel++;
            playerXp -= 100;
            augmentsOwed++;
            playerXpBar.value = playerXp / 100f;
            playerLevelText.text = ("Level: " + playerLevel);
        }

        if (augmentsOwed > 0 && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            augmentManager.RandomAugmentGenerator();
        }
    }

    public void ApplyAugment(string selectedAugment)
    {
        if (selectedAugment == "Attack")
            attack += 5f;
        else if (selectedAugment == "AttackSpeed")
            attackSpeed -= 0.1f;
        else if (selectedAugment == "CritChance")
            critChance += 5;
        else if (selectedAugment == "CritDamage")
            critDamage += 15f;
        else if (selectedAugment == "Armor")
            armor += 3f;
        else if (selectedAugment == "MaxHealth")
            maxHealth += 20f;
        else if (selectedAugment == "Dodge")
            dodge += 4f;
        else if (selectedAugment == "MovementSpeed")
            movementSpeed += 0.5f;
        else if (selectedAugment == "XPGain")
            xpGain += 10f;
        else if (selectedAugment == "GoldGain")
            goldGain += 10f;
    }

    public void GameOver()
    {
        Debug.Log("Game over");
    }
}