using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeSurvivorshipBias", menuName = "Soul Coins/SurvivorshipBias")]
public class SoulCoinSurvivorshipBias : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isSurvivorshipBiasActive = true;
        switch (currentLevel)
        {
            case 1:
                player.survivorshipBiasXP = 1;
                break;
            case 2:
                player.survivorshipBiasXP = 2;
                break;
            case 3:
                player.survivorshipBiasXP = 3;
                break;
        }
    }
}