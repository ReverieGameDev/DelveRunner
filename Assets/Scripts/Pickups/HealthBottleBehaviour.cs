using System.Collections;
using UnityEngine;

public class HealthBottleBehaviour : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool hpTowardsPlayer = false;
    private int hpSpeed = 10;
    private float healFactor = 20;
    public string typeOfHeal;
    private ItemHotbar itemHotbar;
    public InventoryItemData hpSmall;
    public InventoryItemData hpMedium;
    public InventoryItemData hpLarge;
    private InventoryItemData itemObtained;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        itemHotbar = FindFirstObjectByType<ItemHotbar>();
        switch (typeOfHeal)
        {
            case "small":
                itemObtained = hpSmall;
                break;
            case "medium":
                itemObtained = hpMedium;
                break;
            case "large":
                itemObtained = hpLarge;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hpTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * hpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
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

            hpTowardsPlayer = true;

        }
    }
}
