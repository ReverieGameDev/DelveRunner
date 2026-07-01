using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeLegDay", menuName = "Soul Coins/LegDay")]
public class SoulCoinNodeLegDay : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        
        switch (currentLevel)
        {
            case 1:
                player.ModifyStat("movement speed", .03f);
                break;
            case 2:
                player.ModifyStat("movement speed", .05f);
                break;
            case 3:
                player.ModifyStat("movement speed", .08f);
                break;
            case 4:
                player.ModifyStat("movement speed", .12f);
                break;
            case 5:
                player.ModifyStat("movement speed", .16f);
                break;
        }
    }
}