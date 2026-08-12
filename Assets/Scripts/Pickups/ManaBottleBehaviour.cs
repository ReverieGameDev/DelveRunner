using System.Collections;
using UnityEngine;
public class ManaBottleBehaviour : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool manaTowardsPlayer = false;
    private int manaSpeed = 10;
    public string typeOfMana;
    private ItemHotbar itemHotbar;
    public InventoryItemData manaSmall;
    public InventoryItemData manaMedium;
    public InventoryItemData manaLarge;
    private InventoryItemData itemObtained;
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        itemHotbar = FindFirstObjectByType<ItemHotbar>();
        switch (typeOfMana)
        {
            case "small":
                itemObtained = manaSmall;
                break;
            case "medium":
                itemObtained = manaMedium;
                break;
            case "large":
                itemObtained = manaLarge;
                break;
        }
    }
    void Update()
    {
        if (manaTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * manaSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 1.2f)
            {
                Destroy(gameObject);
                itemHotbar.AddToHotbar(itemObtained);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manaTowardsPlayer = true;
        }
    }
}