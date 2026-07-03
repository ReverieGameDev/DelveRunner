using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeStrikeGold", menuName = "Soul Coins/StrikeGold")]
public class SoulCoinNodeStrikeGold : SoulCoinNode
{

    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.strikeGoldActive = true;
        switch (currentLevel)
        {
            case 1:
                player.strikeGoldChance = 5;
                player.strikeGoldAmount = 1;
                break;
            case 2:
                player.strikeGoldChance = 10;
                player.strikeGoldAmount = 1;
                break;
            case 3:
                player.strikeGoldChance = 15;
                player.strikeGoldAmount = 1;
                break;
            case 4:
                player.strikeGoldChance = 20;
                player.strikeGoldAmount = 2;
                break;
            case 5:
                player.strikeGoldChance = 30;
                player.strikeGoldAmount = 3;
                break;
        }
    }
}