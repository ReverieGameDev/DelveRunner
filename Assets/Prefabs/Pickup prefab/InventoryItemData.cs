using UnityEngine;

public abstract class InventoryItemData : ScriptableObject
{
    public string id;
    public Sprite icon;
    public int amount;
    public string itemName;
    public string itemDescription;
    public abstract void Use(PlayerCombat player);
}