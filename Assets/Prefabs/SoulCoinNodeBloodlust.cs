using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


[CreateAssetMenu(fileName = "SoulCoinBloodlust", menuName = "Soul Coins/Bloodlust")]
public class SoulCoinBloodlust: SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.bloodlustIsActive = true;

        switch (currentLevel)
        {
            case 1:
                player.bloodlustDamage = .2f;
                player.bloodlustTime = 4f;
                break;
            case 2:
                player.bloodlustDamage = .3f;
                player.bloodlustTime = 5f;
                break;
            case 3:
                player.bloodlustDamage = .4f;
                player.bloodlustTime = 6f;
                break;
        }
    }
}