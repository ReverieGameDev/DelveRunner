using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeJackpot", menuName = "Soul Coins/Jackpot")]
public class SoulCoinNodeJackpot : SoulCoinNode
{

    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.jackpotActive = true;
        switch (currentLevel)
        {
            case 1:
                player.jackpotChance = 1;
                player.jackpotCritDamage = 30;
                player.jackpotGoldCost = 1;
                break;
            case 2:
                player.jackpotChance = 2;
                player.jackpotCritDamage = 40;
                player.jackpotGoldCost = 2;
                break;
            case 3:
                player.jackpotChance = 4;
                player.jackpotCritDamage = 80;
                player.jackpotGoldCost = 3;
                break;
        }
    }
}