using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    public GameObject inventoryGUI;
    private bool inventoryActive = false;
    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public List<GameObject> inventorySlots = new List<GameObject>();
    public Sprite noItemInventorySprite;
    
    public int inventoryCapacity = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        if (inventoryItems.Count < inventoryCapacity && !itemAddedToInventory)
        {
            InventorySlot newItem = new InventorySlot { item = itemAdded, count = 1 };
            inventoryItems.Add(newItem);
            itemAddedToInventory = true;
        }
        if (inventoryItems.Count == inventoryCapacity && !itemAddedToInventory)
        {
            //display no room message
        }
        InitialInventorySetup();
    }
}
