using UnityEngine;

[CreateAssetMenu(fileName = "aBlitzSoul", menuName = "Augments/aBlitzSoul")]
public class aBlitzSoul : AugmentData
{
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.blitzSoulActive = true;
        playerCombat.blitzToggledOn = true;
        playerCombat.blitzManaCost = 6;
        playerCombat.blitzShockDamage = 4;
        playerCombat.blitzShockDuration = 15f;
        playerCombat.blitzShockTickRate = 3f;
        playerCombat.blitzMaxStacks = 10;
        playerCombat.blitzWeakAutoMult = 0.5f;
    }
}