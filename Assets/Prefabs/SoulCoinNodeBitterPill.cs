using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinBitterPill", menuName = "Soul Coins/BitterPill")]
public class SoulCoinNodeBitterPill : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.bitterPillIsActive = true;

        switch (currentLevel)
        {
            case 1:
                player.bitterPillDamage = .15f;
                player.bitterPillDuration = 3f;
                break;
            case 2:
                player.bitterPillDamage = .22f;
                player.bitterPillDuration = 5f;
                break;
            case 3:
                player.bitterPillDamage = .30f;
                player.bitterPillDuration = 10f;
                break;
        }
    }
}