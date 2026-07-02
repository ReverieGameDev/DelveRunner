using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinFerociousStrikes", menuName = "Soul Coins/FerociousStrikes")]
public class SoulCoinNodeFerociousStrikes : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                player.ModifyStat("attack", .02f);
                break;
            case 2:
                player.ModifyStat("attack", .04f);
                break;
            case 3:
                player.ModifyStat("attack", .07f);
                break;
            case 4:
                player.ModifyStat("attack", .11f);
                break;
            case 5:
                player.ModifyStat("attack", .16f);
                break;
        }
    }
}