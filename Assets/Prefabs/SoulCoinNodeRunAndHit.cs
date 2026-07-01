using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeRunAndHit ", menuName = "Soul Coins/RunAndHit ")]
public class SoulCoinNodeRunAndHit : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isRunAndHitActive = true;
        switch (currentLevel)
        {
            case 1:
                player.runAndHitDamage = .01f;
                player.runAndHitCap = 0.12f;
                break;
            case 2:
                player.runAndHitDamage = .02f;
                player.runAndHitCap = 0.24f;
                break;
            case 3:
                player.runAndHitDamage = .03f;
                player.runAndHitCap = 0.36f;
                break;
        }
    }
}