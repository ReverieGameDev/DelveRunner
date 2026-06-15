using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;
    public string slotType;
    private ItemHotbar itemHotbar;
    private ItemInventory itemInventory;
    public static int draggedFromIndex;
    public static string draggedFromType;
    public static Image dragItemImage;
    public static bool isDragValid;          // NEW: tracks if drag should actually swap
    public Image dragItemImageRef;

    private void Start()
    {
        if (dragItemImageRef != null) dragItemImage = dragItemImageRef;
        itemInventory = FindFirstObjectByType<ItemInventory>();
        itemHotbar = FindFirstObjectByType<ItemHotbar>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragValid = false;

        InventoryItemData sourceItem = (slotType == "Hotbar")
            ? itemHotbar.hotbarItems[slotIndex].item
            : itemInventory.inventoryItems[slotIndex].item;

        if (sourceItem.id == "emptySlot") return;

        isDragValid = true;
        draggedFromIndex = slotIndex;
        draggedFromType = slotType;

        dragItemImage.gameObject.SetActive(true);
        dragItemImage.sprite = sourceItem.icon;
        dragItemImage.rectTransform.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragValid) return;
        dragItemImage.rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragItemImage.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isDragValid) return;
        SwapIndexSlots(draggedFromIndex, draggedFromType, slotIndex, slotType);
    }

    private void SwapIndexSlots(int dragIndex, string dragType, int dropIndex, string dropType)
    {
        if (dragType == "Inventory" && dropType == "Inventory")
        {
            InventorySlot tempHold = itemInventory.inventoryItems[dragIndex];
            itemInventory.inventoryItems[dragIndex] = itemInventory.inventoryItems[dropIndex];
            itemInventory.inventoryItems[dropIndex] = tempHold;
            itemInventory.InitialInventorySetup();
        }
        else if (dragType == "Inventory" && dropType == "Hotbar")
        {
            InventorySlot tempHold = itemInventory.inventoryItems[dragIndex];
            itemInventory.inventoryItems[dragIndex] = itemHotbar.hotbarItems[dropIndex];
            itemHotbar.hotbarItems[dropIndex] = tempHold;
            itemHotbar.CompactHotbar();
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
            itemHotbar.InitialCountHotbarSetup();
            
        }
        else if (dragType == "Hotbar" && dropType == "Inventory")
        {
            InventorySlot tempHold = itemHotbar.hotbarItems[dragIndex];
            itemHotbar.hotbarItems[dragIndex] = itemInventory.inventoryItems[dropIndex];
            itemInventory.inventoryItems[dropIndex] = tempHold;
            itemHotbar.CompactHotbar();
            itemInventory.InitialInventorySetup();
            itemHotbar.InitialHotbarSetup();
            itemHotbar.InitialCountHotbarSetup();
            
        }
        else if (dragType == "Hotbar" && dropType == "Hotbar")
        {
            InventorySlot tempHold = itemHotbar.hotbarItems[dragIndex];
            itemHotbar.hotbarItems[dragIndex] = itemHotbar.hotbarItems[dropIndex];
            itemHotbar.hotbarItems[dropIndex] = tempHold;
            itemHotbar.CompactHotbar();
            itemHotbar.InitialHotbarSetup();
            itemHotbar.InitialCountHotbarSetup();
        }
    }
}