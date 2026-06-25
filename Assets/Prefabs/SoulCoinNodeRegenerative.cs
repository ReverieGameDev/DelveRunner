using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeRegenerative", menuName = "Soul Coins/Regenerative Node")]
public class SoulCoinNodeRegenerative : SoulCoinNode   
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.hpRegen += 3f; // "regen timer in PlayerCombat — tick hpRegen every 5s, combat only."
        player.hpRegenActive = true;
    }
}