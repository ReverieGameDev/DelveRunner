using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    // References
    public PlayerData playerData;
    private PlayerMovement playerMovement;
    private AugmentManager augmentManager;
    public AttackManager attackManager;

    // Combat
    public GameObject closestCurrentEnemy;
    public GameObject autoAttackBullet;

    //Money
    public int playerMoney = 53;

    // Stats
    private float currentPlayerHealth;
    public float attack = 1f;
    public float attackSpeed = 1f;
    public float critChance = 0f;
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

    void Start()
    {
        Debug.Log("PlayerCombat Start - delveLevel: " + delveLevel);
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        augmentManager = FindFirstObjectByType<AugmentManager>();
        attackManager = FindFirstObjectByType<AttackManager>();
        
        currentPlayerHealth = playerData.playerHp;
        playerHpBar.value = 1.0f;

    }

    void Update()
    {
        FindClosestEnemy();
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
    public void DamagePlayer()
    {
        StartCoroutine("IFrames");
        currentPlayerHealth -= 10f;
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
        yield return new WaitForSeconds(1f);
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

        if (playerXp >= 100)
        {
            playerLevel++;
            playerXp = 0;
            playerXpBar.value = 0;
            playerLevelText.text = ("Level: " + playerLevel);
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
            critChance += 5f;
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