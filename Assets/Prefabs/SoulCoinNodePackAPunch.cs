using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinPackAPunch", menuName = "Soul Coins/PackAPunch")]
public class SoulCoinNodePackAPunch : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.packAPunchIsActive = true;
        
        switch (currentLevel)
        {
            case 1:
                player.packAPunchDamagePerItem = .0025f;
                break;
            case 2:
                player.packAPunchDamagePerItem = .0075f;
                break;
            case 3:
                player.packAPunchDamagePerItem = 0.015f;
                break;
        }
    }
}