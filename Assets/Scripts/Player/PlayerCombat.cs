using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

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

    // Combat
    public GameObject closestCurrentEnemy;

    //Money
    public int playerMoney = 0;
    public TextMeshProUGUI playerGold;
    // Stats
    public int currentPlayerHealth;
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
    private string statueStatToMod;

    public List<string> playerStats = new List<string>();

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
        playerGold.text = ": " + playerMoney;
        anim = GetComponent<Animator>();
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
        if (currentPlayerHealth > 0 && currentPlayerHealth < (int)playerData.playerHp)
        {
            int damageHealedInt = (int)Mathf.Round(damageHealed);
            currentPlayerHealth = Mathf.Min(currentPlayerHealth + damageHealedInt, (int)playerData.playerHp);
            playerHpBar.value = currentPlayerHealth / playerData.playerHp;
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
    public void OfferingStatueMods(string statToMod, int goldOffered)
    {
        playerMoney -= goldOffered;
        moneyText.text = ": " + playerMoney;
        switch (statToMod)
        {
            case "attack":
                attack *= 1.15f;
                break;
            case "attack speed":
                attackSpeed *= 1.15f;
                break;
            case "crit chance":
                critChance = (int)(critChance * 1.15f);
                break;
            case "crit damage":
                critDamage *= 1.15f;
                break;
            case "armor":
                armor *= 1.15f;
                break;
            case "max health":
                maxHealth *= 1.15f;
                break;
            case "dodge":
                dodge *= 1.15f;
                break;
            case "movement speed":
                movementSpeed *= 1.15f;
                break;
            case "xp gain":
                xpGain *= 1.15f;
                break;
            case "gold gain":
                goldGain *= 1.15f;
                break;
        }
    }
    public void CurseStatueMods(string blessedStat, string cursedStat)
    {
        switch (blessedStat)
        {
            case "attack":
                attack *= 1.3f;
                break;
            case "attack speed":
                attackSpeed *= 1.3f;
                break;
            case "crit chance":
                critChance = (int)(critChance*1.3f);
                break;
            case "crit damage":
                critDamage *= 1.3f;
                break;
            case "armor":
                armor *= 1.3f;
                break;
            case "max health":
                maxHealth *= 1.3f;
                break;
            case "dodge":
                dodge *= 1.3f;
                break;
            case "movement speed":
                movementSpeed *= 1.3f;
                break;
            case "xp gain":
                xpGain *= 1.3f;
                break;
            case "gold gain":
                goldGain *= 1.3f;
                break;
        }
        switch (cursedStat)
        {
            case "attack":
                attack *= .8f;
                break;
            case "attack speed":
                attackSpeed *= .8f;
                break;
            case "crit chance":
                critChance = (int)(critChance * .8f);
                break;
            case "crit damage":
                critDamage *= .8f;
                break;
            case "armor":
                armor *= .8f;
                break;
            case "max health":
                maxHealth *= .8f;
                break;
            case "dodge":
                dodge *= .8f;
                break;
            case "movement speed":
                movementSpeed *= .8f;
                break;
            case "xp gain":
                xpGain *= .8f;
                break;
            case "gold gain":
                goldGain *= .8f;
                break;
        }
    }

    public void StatueStatMods(string statToMod, float statMod)
    {
        switch (statToMod)
        {
            case "attack":
                attack += statMod;
                break;
            case "attack speed":
                attackSpeed -= statMod;
                break;
            case "crit chance":
                critChance += (int)statMod;
                break;
            case "crit damage":
                critDamage += statMod;
                break;
            case "armor":
                armor += statMod;
                break;
            case "max health":
                maxHealth += statMod;
                break;
            case "dodge":
                dodge += statMod;
                break;
            case "movement speed":
                movementSpeed += statMod;
                break;
            case "xp gain":
                xpGain += statMod;
                break;
            case "gold gain":
                goldGain += statMod;
                break;
        }
    }
}