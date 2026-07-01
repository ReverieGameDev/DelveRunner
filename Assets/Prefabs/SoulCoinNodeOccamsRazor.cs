using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinOccamsRazor", menuName = "Soul Coins/OccamsRazor")]
public class SoulCoinNodeOccamsRazor : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.ModifyStat("consumable effectiveness", -.2f);
        player.ModifyStat("gold gain", -.2f);
        player.ModifyStat("xp gain", .25f);
    }
}