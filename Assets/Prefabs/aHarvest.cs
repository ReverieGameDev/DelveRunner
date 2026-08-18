using UnityEngine;
[CreateAssetMenu(fileName = "aHarvest", menuName = "Augments/aHarvest")]
public class aHarvest : AugmentData
{
    int harvestHealChanceInt;
    int harvestHeal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        harvestHeal = currentLevel;
        playerCombat.harvestActive = true;
        playerCombat.harvestChance = harvestHealChanceInt = 5 + (5 * currentLevel);
        playerCombat.harvestHeal = harvestHeal;
    }
}