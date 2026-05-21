
using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public InventoryItemData item;
    public int count;
}
public class ItemHotbar : MonoBehaviour
{
    public List<InventorySlot> hotbarItems = new List<InventorySlot>();
    public List<GameObject> hotbarSlots = new List<GameObject>();
    public InventoryItemData hpSmall;
    public InventoryItemData manaSmall;
    public InventoryItemData hpLarge;
    public InventoryItemData manaMedium;
    public InventoryItemData emptySlotItem;
    private bool usingItem = false;
    public Sprite noItemSprite;
    private ItemInventory itemInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        itemInventory = FindFirstObjectByType<ItemInventory>();
        InitialHotbarSetup();
        InventorySlot emptySlot = new InventorySlot {item = emptySlotItem, count = 1 };
        hotbarItems.Add(emptySlot);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            HotbarScroll(false);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            HotbarScroll(true);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !usingItem)
        {
            usingItem = true;
            ConsumeItem();
        }

    }
    private void ConsumeItem()
    {
        hotbarItems[0].count--;
        if (hotbarItems[0].count <= 0)
        {
            hotbarItems.RemoveAt(0);
        }
        usingItem = false;
        InitialHotbarSetup();
    }
    public void InitialHotbarSetup()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < hotbarItems.Count)
            {
                hotbarSlots[i].GetComponent<Image>().sprite = hotbarItems[i].item.icon;
            }
            else
            {
                hotbarSlots[i].GetComponent<Image>().sprite = noItemSprite;
            }
        }
    }

    private void HotbarScroll(bool falseDownTrueUp) //which way the hotbar will go 
    {
        if (hotbarItems.Count > 0)
        {

        if (falseDownTrueUp)
        {
            InventorySlot hotBarHolder = hotbarItems[0];
            hotbarItems.RemoveAt(0);
            hotbarItems.Add(hotBarHolder);
            InitialHotbarSetup();
        }
        else
        {
            InventorySlot hotBarHolder = hotbarItems[hotbarItems.Count - 1];
            hotbarItems.RemoveAt(hotbarItems.Count - 1);
            hotbarItems.Insert(0, hotBarHolder);
            InitialHotbarSetup();
        }

        }
    }
    public void AddToHotbar(InventoryItemData itemAdded)
    {
        bool itemHasBeenAdded = false;
        foreach (InventorySlot item in hotbarItems)
        {
            if (item.item.id == itemAdded.id && item.count <4 && !itemHasBeenAdded)
            {
                item.count++;
                itemHasBeenAdded = true;
            }

        }

        if (hotbarItems.Count < 4 && !itemHasBeenAdded)
        {
            InventorySlot newItem = new InventorySlot { item = itemAdded, count = 1 };
            hotbarItems.Add(newItem);
            itemHasBeenAdded = true;
        }
        if (hotbarItems.Count == 4 && !itemHasBeenAdded)
        {
            itemInventory.AddToInventory(itemAdded);
        }
        InitialHotbarSetup();
    }
}
