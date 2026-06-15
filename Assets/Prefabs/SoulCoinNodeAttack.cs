
using UnityEngine;

// Makes the asset creatable from Project > Right-click > Create > Soul Coins > HP Node
[CreateAssetMenu(fileName = "SoulCoinNodeAttack", menuName = "Soul Coins/Attack Node")]
public class SoulCoinNodeAttack : SoulCoinNode   // ← colon = inherits from
{
    public float attackPerLevel = 5f;   // tweak in Inspector per asset

    // 'override' tells the compiler: "I'm providing the body for the abstract method"
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.attack += attackPerLevel * currentLevel;
    }
}