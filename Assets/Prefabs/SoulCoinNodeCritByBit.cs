using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeCritByBit ", menuName = "Soul Coins/CritByBit ")]
public class SoulCoinNodeCritByBit : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                player.ModifyStat("crit chance", .03f);
                break;
            case 2:
                player.ModifyStat("crit chance", .07f);
                break;
            case 3:
                player.ModifyStat("crit chance", .11f);
                break;
            case 4:
                player.ModifyStat("crit chance", .15f);
                break;
            case 5:
                player.ModifyStat("crit chance", .20f);
                break;
        }
    }
}