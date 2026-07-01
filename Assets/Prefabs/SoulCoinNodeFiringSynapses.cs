using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinFiringSynapses", menuName = "Soul Coins/FiringSynapses")]
public class SoulCoinNodeFiringSynapses : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.ModifyStat("xp gain", .1f);
    }
}