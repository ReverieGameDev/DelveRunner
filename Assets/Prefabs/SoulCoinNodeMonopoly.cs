using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeMonopoly", menuName = "Soul Coins/Monopoly")]
public class SoulCoinNodeMonopoly : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                player.ModifyStat("gold gain",0.03f);
                break;
            case 2:
                player.ModifyStat("gold gain", 0.05f);
                break;
            case 3:
                player.ModifyStat("gold gain", 0.08f);
                break;
            case 4:
                player.ModifyStat("gold gain", 0.12f);
                break;
            case 5:
                player.ModifyStat("gold gain", 0.16f);
                break;
        }
    }
}