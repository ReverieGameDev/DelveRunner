using UnityEngine;

// Makes the asset creatable from Project > Right-click > Create > Soul Coins > HP Node
[CreateAssetMenu(fileName = "SoulCoinNodeSoulSiphon", menuName = "Soul Coins/Soul Siphon Node")]
public class SoulCoinNodeSoulSiphon : SoulCoinNode   // ← colon = inherits from
{
    // 'override' tells the compiler: "I'm providing the body for the abstract method"
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.soulSiphonLevel = currentLevel;
    }
}