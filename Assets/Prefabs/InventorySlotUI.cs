using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;//to show which slot in the list we're dealing with. but there are 2 lists see below
    public string slotType;//this will tell us which list we're dealing with, hotbar or inv
    private ItemHotbar itemHotbar;
    private ItemInventory itemInventory;
    public static int draggedFromIndex;
    public static string draggedFromType;

    private void Start()
    {
        itemInventory = FindFirstObjectByType<ItemInventory>();
        itemHotbar = FindFirstObjectByType<ItemHotbar>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedFromIndex = slotIndex;
        draggedFromType = slotType;
    }
    public void OnDrop(PointerEventData eventData)
    {
        int droppedOnIndex = slotIndex;
        string droppedOnType = slotType;
        SwapIndexSlots(draggedFromIndex, draggedFromType, droppedOnIndex, droppedOnType);
    }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }
    private void SwapIndexSlots(int dragIndex, string dragType, int dropIndex, string dropType)
    {
        if (dragType == "Inventory" && dropType == "Inventory")
        {
            InventorySlot tempHold = itemInventory.inventoryItems[dragIndex];
            itemInventory.inventoryItems[dragIndex] = itemInventory.inventoryItems[dropIndex];
            itemInventory.inventoryItems[dropIndex] = tempHold;
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
        }
        else if (dragType == "Inventory" && dropType == "Hotbar")
        {
            InventorySlot tempHold = itemInventory.inventoryItems[dragIndex];
            itemInventory.inventoryItems[dragIndex] = itemHotbar.hotbarItems[dropIndex];
            itemHotbar.hotbarItems[dropIndex] = tempHold;
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
        }
        else if (dragType == "Hotbar" && dropType == "Inventory")
        {
            InventorySlot tempHold = itemHotbar.hotbarItems[dragIndex];
            itemHotbar.hotbarItems[dragIndex] = itemInventory.inventoryItems[dropIndex];
            itemInventory.inventoryItems[dropIndex] = tempHold;
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
        }
        else if (dragType == "Hotbar" && dropType == "Hotbar")
        {
            InventorySlot tempHold = itemHotbar.hotbarItems[dragIndex];
            itemHotbar.hotbarItems[dragIndex] = itemHotbar.hotbarItems[dropIndex];
            itemHotbar.hotbarItems[dropIndex] = tempHold;
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
        }
    }
}