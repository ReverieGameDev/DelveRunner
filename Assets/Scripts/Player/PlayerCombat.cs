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
    public float attackSpeed = 0.5f;
    public float attackDamage = 25f;
    public float armor = 0f;
    public float magicResist = 0f;
    public float movementSpeed = 5f;
    public float size = 1f;

    // UI
    public Slider playerHpBar;
    public Slider playerXpBar;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI moneyText;

    // State
    public bool iFrames = false;
    public int playerXp;
    public int playerLevel = 1;
    public int delveLevel = 1;

    void Start()
    {
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
        if (selectedAugment == "AttackSpeed")
        {
            attackSpeed -= 0.4f; // INSANE fire rate
        }
        else if (selectedAugment == "Attack")
        {
            attackDamage += 200f; // ONE SHOT EVERYTHING
        }
        else if (selectedAugment == "Armor")
        {
            armor += 100f;
        }
        else if (selectedAugment == "MagicResist")
        {
            magicResist += 100f;
        }
        else if (selectedAugment == "MovementSpeed")
        {
            movementSpeed += 10f; // ZOOM ZOOM
        }
        else if (selectedAugment == "Size")
        {
            size += 1.5f; // MASSIVE
            transform.localScale = Vector3.one * size;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game over");
    }
}