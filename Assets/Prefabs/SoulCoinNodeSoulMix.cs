using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinSoulMix ", menuName = "Soul Coins/SoulMix")]
public class SoulCoinNodeSoulMix : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.soulMixActive = true;
        switch (currentLevel)
        {
            case 1: player.soulMixPercent = .03f; player.soulMixCap = 5; break;
            case 2: player.soulMixPercent = .05f; player.soulMixCap = 8; break;
            case 3: player.soulMixPercent = .08f; player.soulMixCap = 12; break;
            case 4: player.soulMixPercent = .12f; player.soulMixCap = 18; break;
            case 5: player.soulMixPercent = .20f; player.soulMixCap = 30; break;
        }
    }
}