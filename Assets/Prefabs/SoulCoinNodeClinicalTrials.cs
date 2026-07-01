using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinClinicalTrials", menuName = "Soul Coins/ClinicalTrials")]
public class SoulCoinNodeClinicalTrials : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.ModifyStat("consumable effectiveness", .15f);
    }
}