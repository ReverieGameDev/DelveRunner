using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeHindsightBias", menuName = "Soul Coins/HindsightBias")]
public class SoulCoinNodeHindsightBias : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isHindsightBiasActive = true;
        switch (currentLevel)
        {
            case 1:
                player.hindsightBiasReturnTime = 20;
                player.hindsightBiasHealthReturn = 1;
                break;
            case 2:
                player.hindsightBiasReturnTime = 18;
                player.hindsightBiasHealthReturn = 2;
                break;
            case 3:
                player.hindsightBiasReturnTime = 16;
                player.hindsightBiasHealthReturn = 2;
                break;
            case 4:
                player.hindsightBiasReturnTime = 14;
                player.hindsightBiasHealthReturn = 3;
                break;
            case 5:
                player.hindsightBiasReturnTime = 10;
                player.hindsightBiasHealthReturn = 4;
                break;
        }
    }
}