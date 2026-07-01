using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeDauntless", menuName = "Soul Coins/Dauntless")]
public class SoulCoinNodeDauntless : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        int healthDiff = (int)((player.maxHealth - (int)player.baseMaxHealth)/30);
        if (healthDiff > 0)
        {
            for (int i = 0; i < Mathf.Min(healthDiff,3); i++)
            {
                player.statusResist += .1f;
            }
            if (healthDiff > 2)
            {
                player.statusResist += .05f;
            }
        }
    }
}