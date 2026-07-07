using Unity.VisualScripting;
using UnityEngine;

// Makes the asset creatable from Project > Right-click > Create > Soul Coins > HP Node
[CreateAssetMenu(fileName = "SoulCoinNodeHP", menuName = "Soul Coins/HP Node")]
public class SoulCoinNodeHP : SoulCoinNode   // ← colon = inherits from
{
    public float hpPerLevel = 20f;   // tweak in Inspector per asset

    // 'override' tells the compiler: "I'm providing the body for the abstract method"
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.maxHealth += hpPerLevel * currentLevel;
    }
}