using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinAchillesHeel", menuName = "Soul Coins/AchillesHeel")]
public class SoulCoinAchillesHeel : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.achillesHeelIsActive = true;

        switch (currentLevel)
        {
            case 1:
                player.achillesHeelChance = 5f;
                player.achillesHeelDamage = 1.75f;
                break;
            case 2:
                player.achillesHeelChance = 8f;
                player.achillesHeelDamage = 2f;
                break;
            case 3:
                player.achillesHeelChance = 10f;
                player.achillesHeelDamage = 2.25f;
                break;
        }
    }
}