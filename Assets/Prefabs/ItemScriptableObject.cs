using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "DelveRunner/Item")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public Sprite icon;
    public int amount;
}