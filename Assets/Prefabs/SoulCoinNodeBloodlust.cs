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
                player.bloodlustDamage = .1f;
                player.bloodlustTime = 6f;
                break;
            case 2:
                player.bloodlustDamage = .15f;
                player.bloodlustTime = 6f;
                break;
            case 3:
                player.bloodlustDamage = .2f;
                player.bloodlustTime = 6f;
                break;
        }
    }
}