using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    public GameObject inventoryGUI;
    private bool inventoryActive = false;
    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public List<GameObject> inventorySlots = new List<GameObject>();
    public Sprite noItemInventorySprite;
    private ItemHotbar itemHotbar;
    public List<TextMeshProUGUI> itemAmounts = new List<TextMeshProUGUI>();


    public int inventoryCapacity = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemHotbar = FindFirstObjectByType<ItemHotbar>();
        for (int i = 0; i < inventoryCapacity; i++)
        {
            InventorySlot slot = new InventorySlot();
            slot.item = itemHotbar.emptySlotItem;
            slot.count = 0;
            inventoryItems.Add(slot);
        }
        InitialInventorySetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!FindFirstObjectByType<Enemy>() && !inventoryActive)
            {
                Time.timeScale = 0;
                inventoryActive = !inventoryActive;
                inventoryGUI.SetActive(inventoryActive);
                InitialInventorySetup();
            }
            else if (inventoryActive)
            {
                Time.timeScale = 1;
                inventoryActive = !inventoryActive;
                inventoryGUI.SetActive(inventoryActive);
            }
            
        }
    }
    public void InitialCountInventorySetup()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            itemAmounts[i].text = inventoryItems[i].item.id != "emptySlot"
                ? "" + inventoryItems[i].count
                : "";
        }
    }
    public void InitialInventorySetup()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (i < inventoryItems.Count)
            {
                inventorySlots[i].GetComponent<Image>().sprite = inventoryItems[i].item.icon;

            }
            else
            {
                inventorySlots[i].GetComponent<Image>().sprite = noItemInventorySprite;
            }


        }
        InitialCountInventorySetup();
    }

    public void AddToInventory(InventoryItemData itemAdded)
    {
        bool itemAddedToInventory = false;
        foreach (InventorySlot item in inventoryItems)
        {
            if (item.item.id == itemAdded.id && item.count < inventoryCapacity && !itemAddedToInventory)
            {
                item.count++;
                itemAddedToInventory = true;
            }

        }

        if (!itemAddedToInventory)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item.id == "emptySlot")
                {
                    inventoryItems[i].item = itemAdded;
                    inventoryItems[i].count = 1;
                    itemAddedToInventory = true;
                    break;
                }
            }
        }
        if (inventoryItems.Count == inventoryCapacity && !itemAddedToInventory)
        {
            //display no room message
        }
        InitialInventorySetup();
    }
}
