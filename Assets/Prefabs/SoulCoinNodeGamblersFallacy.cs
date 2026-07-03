using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeGamblersFallacy", menuName = "Soul Coins/GamblersFallacy")]
public class SoulCoinNodeGamblersFallacy : SoulCoinNode
{

    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.gamblersFallacyActive = true;
        player.gamblersFallacyPayout = .25f;
    }
}