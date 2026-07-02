using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


[CreateAssetMenu(fileName = "SoulCoinDoOrDie", menuName = "Soul Coins/DoOrDie")]
public class SoulCoinDoOrDie : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.doOrDieIsActive = true;

        switch (currentLevel)
        {
            case 1:
                player.doOrDieHpThreshold = .15f;
                player.doOrDieAS = .3f;
                break;
            case 2:
                player.doOrDieHpThreshold = .20f;
                player.doOrDieAS = .45f;
                break;
            case 3:
                player.doOrDieHpThreshold = .35f;
                player.doOrDieAS = .65f;
                break;
        }
    }
}