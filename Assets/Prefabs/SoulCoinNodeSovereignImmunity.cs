using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeSovereignImmunity", menuName = "Soul Coins/SovereignImmunity")]
public class SoulCoinNodeSovereignImmunity : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isSovereignImmunityActive = true;
        switch (currentLevel)
        {
            case 1:
                player.sovereignImmunityCooldown = 85;
                break;
            case 2:
                player.sovereignImmunityCooldown = 75;
                break;
            case 3:
                player.sovereignImmunityCooldown = 65;
                break;
            case 4:
                player.sovereignImmunityCooldown = 55;
                break;
            case 5:
                player.sovereignImmunityCooldown = 40;
                break;
        }
    }
}