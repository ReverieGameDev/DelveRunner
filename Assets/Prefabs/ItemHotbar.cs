using System;
using System.Collections.Generic;
using TMPro;
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
    public InventorySlot emptySlot;
    public TextMeshProUGUI slot0Count;
    public TextMeshProUGUI slot1Count;
    public TextMeshProUGUI slot2Count;
    public TextMeshProUGUI slot3Count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        itemInventory = FindFirstObjectByType<ItemInventory>();
        
        emptySlot = new InventorySlot {item = emptySlotItem, count = 1 };
        hotbarItems.Add(emptySlot);
        hotbarItems.Add(emptySlot);
        hotbarItems.Add(emptySlot);
        hotbarItems.Add(emptySlot);
        InitialHotbarSetup();
        InitialCountHotbarSetup();
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
    public void CompactHotbar()
    {
        List<InventorySlot> tempHotbar = new List<InventorySlot>();
        foreach (InventorySlot item in hotbarItems)
        {
            if (item.item.id != "emptySlot") tempHotbar.Add(item);
        }
        hotbarItems = tempHotbar;
        while (hotbarItems.Count < 4) hotbarItems.Add(emptySlot);
    }
    private void ConsumeItem()
    {
        if (hotbarItems[0].item != emptySlotItem)
        {
            hotbarItems[0].count--;
            hotbarItems[0].item.Use(PlayerCombat.Instance);
            if (PlayerCombat.Instance.isSchrodingersCatActive && PlayerCombat.Instance.SchrodingersCat())
            {
                hotbarItems[0].item.Use(PlayerCombat.Instance);
            }
            if (PlayerCombat.Instance.isInsiderTradingActive && PlayerCombat.Instance.InsiderTrading())
            {
                PlayerCombat.Instance.ModifyGoldValue("pickup",PlayerCombat.Instance.insiderTradingGoldAmount);
            }
            if (hotbarItems[0].count <= 0)
            {
                hotbarItems.RemoveAt(0);
                hotbarItems.Add(emptySlot);
            }
            usingItem = false;
            InitialHotbarSetup();
            InitialCountHotbarSetup();
        }
        //else, popup message = no item selected
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

    public void InitialCountHotbarSetup()
    {
        if (hotbarItems[0].item.id != "emptySlot")
        {
            slot0Count.text = "" + hotbarItems[0].count;
        }
        else slot0Count.text = "";
        if (hotbarItems[1].item.id != "emptySlot")
        {
            slot1Count.text = "" + hotbarItems[1].count;
        }
        else slot1Count.text = "";
        if (hotbarItems[2].item.id != "emptySlot")
        {
            slot2Count.text = "" + hotbarItems[2].count;
        }
        else slot2Count.text = "";
        if (hotbarItems[3].item.id != "emptySlot")
        {
            slot3Count.text = "" + hotbarItems[3].count;
        }
        else slot3Count.text = "";
    }

    private void HotbarScroll(bool falseDownTrueUp) //which way the hotbar will go 
    {
        List<InventorySlot> tempHotbar = new List<InventorySlot>();
        foreach (InventorySlot item in hotbarItems)
        {
            if (item.item.id != "emptySlot")
            {
                tempHotbar.Add(item);
            }
        }
        if (tempHotbar.Count <= 1) return;
        if (falseDownTrueUp)
        {
            InventorySlot hotBarHolder = tempHotbar[0];
            tempHotbar.RemoveAt(0);
            tempHotbar.Add(hotBarHolder);
            hotbarItems = tempHotbar;
            while (hotbarItems.Count < 4) hotbarItems.Add(emptySlot);
            InitialHotbarSetup();
            InitialCountHotbarSetup();
        }
        else
        {
            InventorySlot hotBarHolder = tempHotbar[tempHotbar.Count - 1];
            tempHotbar.RemoveAt(tempHotbar.Count - 1);
            tempHotbar.Insert(0, hotBarHolder);
            hotbarItems = tempHotbar;
            while (hotbarItems.Count < 4) hotbarItems.Add(emptySlot);
            InitialHotbarSetup();
            InitialCountHotbarSetup();
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
        if (!itemHasBeenAdded)
        {
            for (int i = 0; i < 4; i++)
            {
                if (hotbarItems[i].item.id == "emptySlot" && !itemHasBeenAdded)
                {

                    InventorySlot newItem = new InventorySlot { item = itemAdded, count = 1 };
                    hotbarItems.RemoveAt(i);
                    hotbarItems.Insert(i, newItem);
                    itemHasBeenAdded = true;
                }
            }
        }

        if (!itemHasBeenAdded)
        {
            itemInventory.AddToInventory(itemAdded);
        }
        InitialHotbarSetup();
        InitialCountHotbarSetup();
    }
}
