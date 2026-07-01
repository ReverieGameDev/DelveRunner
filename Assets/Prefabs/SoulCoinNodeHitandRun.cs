using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeHitandRun", menuName = "Soul Coins/HitandRun")]
public class SoulCoinHitandRun : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isHitandRunActive = true;
        switch (currentLevel)
        {
            case 1:
                player.hitandRunValue = .15f;
                player.hitandRunTime = 1.1f;
                break;
            case 2:
                player.hitandRunValue = .20f;
                player.hitandRunTime = 2.2f;
                break;
            case 3:
                player.hitandRunValue = .30f;
                player.hitandRunTime = 3.3f;
                break;
        }
    }
}