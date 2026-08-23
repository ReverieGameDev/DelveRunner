using UnityEngine;

[CreateAssetMenu(fileName = "aConduction", menuName = "Augments/aConduction")]
public class aConduction : AugmentData
{
    int aShockChance;
    int currentStaticLevel;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aConductionActive = true;
        playerCombat.aConductionManaPerTick = 2*currentLevel;
    }
}