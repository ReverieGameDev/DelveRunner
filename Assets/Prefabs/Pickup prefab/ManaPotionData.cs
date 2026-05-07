using UnityEngine;

[CreateAssetMenu(menuName = "DelveRunner/Items/ManaPotion")]
public class ManaPotionData : InventoryItemData
{
    public override void Use(PlayerCombat player)
    {
        player.currentPlayerMana = Mathf.Min(player.currentPlayerMana + amount, player.playerManaBase);
    }
}