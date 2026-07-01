using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeDynamicDensity", menuName = "Soul Coins/DynamicDensity")]
public class SoulCoinNodeDynamicDensity : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isDynamicDensityActive = true;
    }
}