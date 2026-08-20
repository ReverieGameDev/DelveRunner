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
                player.jackpotChance = 5;
                player.jackpotCritDamage = 8;
                player.jackpotGoldCost = 1;
                break;
            case 2:
                player.jackpotChance = 10;
                player.jackpotCritDamage = 12;
                player.jackpotGoldCost = 2;
                break;
            case 3:
                player.jackpotChance = 15;
                player.jackpotCritDamage = 20;
                player.jackpotGoldCost = 3;
                break;
        }
    }
}