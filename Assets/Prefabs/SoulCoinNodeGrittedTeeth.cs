using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeGrittedTeeth", menuName = "Soul Coins/Gritted Teeth")]
public class SoulCoinNodeGrittedTeeth : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                player.ModifyStat("armor", 1);
                break;
            case 2:
                player.ModifyStat("armor", 2);
                break;
            case 3:
                player.ModifyStat("armor", 4);
                break;
            case 4:
                player.ModifyStat("armor", 7);
                break;
            case 5:
                player.ModifyStat("armor", 10);
                break;
        }
    }
}