using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinSchrodingersCat", menuName = "Soul Coins/SchrodingersCat")]
public class SoulCoinNodeSchrodingersCat : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isSchrodingersCatActive = true;
        player.schrodingersCatCurrentLevel = currentLevel;
    }
}