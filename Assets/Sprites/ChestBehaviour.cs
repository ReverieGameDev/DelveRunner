using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestBehaviour : MonoBehaviour
{
    public string chestRarity;
    public Dictionary<string, int> lootTable = new Dictionary<string, int>();
    private int totalWeight;
    private int randomRoll;
    private List<string> winningItems = new List<string>();
    private int chestRolls;
    public List<GameObject> droppableItems = new List<GameObject>();
    private bool isLooted = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lootTable.Add("HPSmall", 20);
        lootTable.Add("XP", 20);
        lootTable.Add("HPMedium", 10);
        lootTable.Add("HPLarge", 5);
        lootTable.Add("LargeEmber", 20);
        //lootTable.Add("hiddenVaultKey", 1);

        if (chestRarity == "uncommon")
        {
            lootTable["HPSmall"] *= 1;
            lootTable["XP"] *= 1;
            lootTable["HPMedium"] *= 2;
            lootTable["HPLarge"] *= 2;
        }
        if (chestRarity == "rare")
        {
            lootTable["HPSmall"] *= 1;
            lootTable["XP"] *= 1;
            lootTable["HPMedium"] *= 3;
            lootTable["HPLarge"] *= 4;
            //lootTable["fullLevelUp"] *= 2;
        }
        switch (chestRarity)
        {
            case "common":
                chestRolls = 1;
                break; 

            case "uncommon":
                chestRolls = 2;
                break;

            case "rare":
                chestRolls = 3;
                break;

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && !isLooted)
            {
                isLooted = true;
                LootRoll();
            }
        }
    }
    private void LootRoll()
    {
        winningItems.Clear();
        totalWeight = 0;
        foreach (int val in lootTable.Values)
        {
            totalWeight += val;
        }


        for (int i = 0; i < chestRolls; i++)
        {
            randomRoll = Random.Range(0, totalWeight);
            foreach (string item in lootTable.Keys)
            {
                randomRoll -= lootTable[item];
                if (randomRoll < 0)
                {
                    winningItems.Add(item);
                    break;
                }
                }
        }
        for (int i = 0; i < winningItems.Count; i++)
        {
            for (int x = 0; x < droppableItems.Count; x++)
            {
                if (winningItems[i] == droppableItems[x].name)
                {
                    float randomX = Random.Range(-3, 3);
                    float randomY = Random.Range(-3, 3);
                    Instantiate(droppableItems[x], new Vector2(transform.position.x + randomX, transform.position.y + randomY), Quaternion.identity);
                }
            }
            
        }
        for (int i = 0; i < winningItems.Count; i++)
        {
            Debug.Log("Item " + i + ": " + winningItems[i]);
        }
        OpenChestAnimation();
    }

    private void OpenChestAnimation()
    {
        anim.speed = 1;
    }

    public void DestroyChest()
    {
        Destroy(gameObject);
    }
}
