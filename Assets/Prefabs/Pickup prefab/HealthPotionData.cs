using UnityEngine;

[CreateAssetMenu(menuName = "DelveRunner/Items/HealthPotion")]
public class HealthPotionData : InventoryItemData
{
    public override void Use(PlayerCombat player)
    {
        player.HealPlayer(amount*player.consumableEffectiveness);
    }
}