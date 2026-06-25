using UnityEngine;
[CreateAssetMenu(fileName = "SoulCoinNodeRampingRegen", menuName = "Soul Coins/Ramping Regen Node")]
public class SoulCoinNodeRampingRegen : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.rampingRegenValue = currentLevel;
        player.rampingRegenActive = true;
    }
}
