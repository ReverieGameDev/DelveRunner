using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    public GameObject bloodHealPrefab;
    // References
    public PlayerData playerData;
    private PlayerMovement playerMovement;
    private AugmentManager augmentManager;
    public AttackManager attackManager;
    private Animator anim;
    private EmberSystem emberSystem;

    public GameObject gameOverScreen;

    // Combat
    public GameObject closestCurrentEnemy;

    //Money
    public int playerMoney = 100;
    public TextMeshProUGUI playerGold;
    // Stats
    public int currentPlayerHealth;
    public float attack = 1f;
    public float attackSpeed = 1f;
    public int critChance = 5;
    public float critDamage = 1.5f;
    public float armor = 0f;
    public float maxHealth = 100f;// max health DURING delves
    public float dodge = 1f;
    public float movementSpeed = 5f;
    public float xpGain = 1f;
    public float goldGain = 1f;
    public float playerManaBase = 100;
    public float currentPlayerMana = 100f;
    private string statueStatToMod;
    public float baseMaxHealth; // max health BETWEEN delves, after soul mix calcs.
    public float hpRegen;
    private bool hpIsRegenning = false;

    public int totalSiphonKills;
    public int siphonCounter;
    public int soulSiphonLevel;
    public float rampingRegenValue;
    private float rampingRegenCounter = 1;

    public List<string> playerStats = new List<string>();

    // UI
    public Slider playerHpBar;
    public Slider playerManaBar;
    public Slider playerXpBar;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerHpBarNumber;
    public TextMeshProUGUI playerManaBarNumber;

    // State
    public bool iFrames = false;
    public int playerXp;
    public int playerLevel = 1;
    public int delveLevel = 0;
    public int augmentsOwed = 0;
    public bool hpRegenActive = false;
    public bool rampingRegenActive = false;
    public bool bloodSoulBarrierActive = false;
    public int bloodSoulBarrierValue;
    void Awake()
    {
        Instance = this;
        playerStats.Add("attack");
        playerStats.Add("attack speed");
        playerStats.Add("crit chance");
        playerStats.Add("crit damage");
        playerStats.Add("armor");
        playerStats.Add("max health");
        playerStats.Add("dodge");
        playerStats.Add("movement speed");
        playerStats.Add("xp gain");
        playerStats.Add("gold gain");
    }

    void Start()
    {
        if (playerGold != null) playerGold.text = ": " + playerMoney;
        
        anim = GetComponent<Animator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        augmentManager = FindFirstObjectByType<AugmentManager>();
        attackManager = FindFirstObjectByType<AttackManager>();
        emberSystem = FindFirstObjectByType<EmberSystem>();

        currentPlayerHealth = (int)maxHealth;
        currentPlayerMana = playerManaBase;
        playerHpBarNumber.text = currentPlayerHealth + " / " + (int)maxHealth;
        playerManaBarNumber.text = currentPlayerMana + " / " + playerManaBase;
        if (playerManaBar != null) playerManaBar.value = 1.0f;
        if (playerHpBar != null) playerHpBar.value = 1.0f; 
    }

    private void Update()
    {
        if (emberSystem.aliveEnemies > 0 && hpRegenActive && !hpIsRegenning && currentPlayerHealth < maxHealth)
        {
            StartCoroutine("HpRegen");
        }
        if (playerManaBar != null) playerManaBar.value = currentPlayerMana / playerManaBase;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = FindFirstObjectByType<EnemySpawnDetector>().transform.position;
        }
    }

    IEnumerator HpRegen()
    {

        if (currentPlayerHealth >= maxHealth)
        {
            hpIsRegenning = false;
            rampingRegenCounter = 1;
            yield break;
        }
        hpIsRegenning = true;
        if (rampingRegenActive)
        {
            int healAmount = (int)hpRegen + (int)(rampingRegenValue * Mathf.Sqrt(rampingRegenCounter) / 2f);
            healAmount = Mathf.Min(healAmount, 14);
            currentPlayerHealth += healAmount;
        }
        else if (!rampingRegenActive)
        {
            currentPlayerHealth += (int)hpRegen;
        }
        yield return new WaitForSeconds(5f);
        rampingRegenCounter++;
        if (emberSystem.aliveEnemies > 0 && hpRegenActive)
        {
            StartCoroutine("HpRegen");
        }
        else if (emberSystem.aliveEnemies<=0)
        {
            hpIsRegenning = false;
            rampingRegenCounter=1;
        }
    }


    public void OnEnemyKilled()
    {
        if (bloodSoulBarrierActive && currentPlayerHealth < (int)maxHealth)
        {
            HealPlayer(bloodSoulBarrierValue);
        }
        if (soulSiphonLevel > 0)
        {
            if (siphonCounter < 3)
            {
                siphonCounter++;
            }
            if (siphonCounter >= 3)
            {
                siphonCounter = 0;
                totalSiphonKills++;
                maxHealth++;
                currentPlayerHealth++;
                Debug.Log("health should be added, soul siphon level: " + soulSiphonLevel);
                playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
            }
        }
    }

    public int CalcWeaponDamage(float damage, out bool crit)
    {
        int critRoll = Random.Range(0, 101);
        int processedDamage = 0;
       

        if (critRoll < critChance)
        {
            processedDamage = (int)(Mathf.Round(damage * attack * critDamage));
            crit = true;
        }
        else
        {
            crit = false;
            processedDamage = (int)(Mathf.Round(damage * attack));
        }
        if (emberSystem != null && emberSystem.emberAmount <= 0)
        processedDamage = (int)(processedDamage * 0.85f);
        return processedDamage;
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
        int dodgeChance = Random.Range(0, 101);
        if (dodgeChance <= dodge)
        {
            return;
        }
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        if (emberSystem != null && emberSystem.emberAmount <= 0)
            damageTakenInt = (int)(damageTakenInt * 1.15f);
        if (iFrames) return;
        StartCoroutine("IFrames");
        float reduction = armor / (armor + 100f);
        int finalDamage = (int)Mathf.Round(damageTakenInt * (1f - reduction));
        currentPlayerHealth -= finalDamage;
        playerHpBar.value = currentPlayerHealth / maxHealth;
        playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
        if (currentPlayerHealth <= 0)
        {
            anim.SetTrigger("Death");

            GameOver();
        }
    }

    public void BloodHeal(int damageHealed)
    {
        Instantiate(bloodHealPrefab, transform.position, Quaternion.identity);
        HealPlayer(damageHealed);
    }
    public void HealPlayer(float damageHealed)
    {
        if (currentPlayerHealth > 0 && currentPlayerHealth < (int)maxHealth)
        {
            int damageHealedInt = (int)Mathf.Round(damageHealed);
            currentPlayerHealth = Mathf.Min(currentPlayerHealth + damageHealedInt, (int)maxHealth);
            playerHpBar.value = currentPlayerHealth / maxHealth;
            playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
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
        anim.SetTrigger("Hurt");
        return null;
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
            playerLevelText.text = (""+playerLevel);
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
            attack *= 1.05f;
        else if (selectedAugment == "AttackSpeed")
            attackSpeed *= 0.95f;
        else if (selectedAugment == "CritChance")
            critChance += 10;
        else if (selectedAugment == "CritDamage")
            critDamage *= 1.10f;
        else if (selectedAugment == "Armor")
            armor += 10f;
        else if (selectedAugment == "MaxHealth")
            maxHealth *= 1.05f;
        else if (selectedAugment == "Dodge")
            dodge += 4f;
        else if (selectedAugment == "MovementSpeed")
            movementSpeed *= 1.05f;
        else if (selectedAugment == "XPGain")
            xpGain *= 1.05f;
        else if (selectedAugment == "GoldGain")
            goldGain *= 1.10f;
    }

    public void GameOver()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("TradingHub");
        Time.timeScale = 1;
    }
    public void OfferingStatueMods(string statToMod, int goldOffered)
    {
        playerMoney -= goldOffered;
        moneyText.text = ": " + playerMoney;
        switch (statToMod)
        {
            case "attack":
                attack *= 1.10f;
                break;
            case "attack speed":
                attackSpeed *= 0.90f;
                break;
            case "crit chance":
                critChance += 10;
                break;
            case "crit damage":
                critDamage *= 1.10f;
                break;
            case "armor":
                armor += 15f;
                break;
            case "max health":
                maxHealth *= 1.10f;
                break;
            case "dodge":
                dodge += 5f;
                break;
            case "movement speed":
                movementSpeed *= 1.10f;
                break;
            case "xp gain":
                xpGain *= 1.10f;
                break;
            case "gold gain":
                goldGain *= 1.10f;
                break;
        }
    }

    public void CurseStatueMods(string blessedStat, string cursedStat)
    {
        switch (blessedStat)
        {
            case "attack":
                attack *= 1.04f;
                break;
            case "attack speed":
                attackSpeed *= 0.96f;
                break;
            case "crit chance":
                critChance += 4;
                break;
            case "crit damage":
                critDamage *= 1.04f;
                break;
            case "armor":
                armor += 8f;
                break;
            case "max health":
                maxHealth *= 1.04f;
                break;
            case "dodge":
                dodge += 2f;
                break;
            case "movement speed":
                movementSpeed *= 1.04f;
                break;
            case "xp gain":
                xpGain *= 1.04f;
                break;
            case "gold gain":
                goldGain *= 1.04f;
                break;
        }
        switch (cursedStat)
        {
            case "attack":
                attack *= 0.96f;
                break;
            case "attack speed":
                attackSpeed *= 1.04f;
                break;
            case "crit chance":
                critChance -= 4;
                break;
            case "crit damage":
                critDamage *= 0.96f;
                break;
            case "armor":
                armor += 8f;
                break;
            case "max health":
                maxHealth *= 0.96f;
                break;
            case "dodge":
                dodge -= 2f;
                break;
            case "movement speed":
                movementSpeed *= 0.96f;
                break;
            case "xp gain":
                xpGain *= 0.96f;
                break;
            case "gold gain":
                goldGain *= 0.96f;
                break;
        }
    }

    public void StatueStatMods(string statToMod, float statMod)
    {
        switch (statToMod)
        {
            case "attack":
                attack *= (statMod > 0) ? 1.07f : 0.93f;
                break;
            case "attack speed":
                attackSpeed *= (statMod > 0) ? 0.93f : 1.07f;
                break;
            case "crit chance":
                critChance += (statMod > 0) ? 7 : -7;
                break;
            case "crit damage":
                critDamage *= (statMod > 0) ? 1.07f : 0.93f;
                break;
            case "armor":
                armor += (statMod > 0) ? 12f : -12f;
                break;
            case "max health":
                maxHealth *= (statMod > 0) ? 1.07f : 0.93f;
                break;
            case "dodge":
                dodge += (statMod > 0) ? 4f : -4f;
                break;
            case "movement speed":
                movementSpeed *= (statMod > 0) ? 1.07f : 0.93f;
                break;
            case "xp gain":
                xpGain *= (statMod > 0) ? 1.07f : 0.93f;
                break;
            case "gold gain":
                goldGain *= (statMod > 0) ? 1.07f : 0.93f;
                break;
        }
    }
}