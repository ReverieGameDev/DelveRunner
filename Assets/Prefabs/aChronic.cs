using UnityEngine;
[CreateAssetMenu(fileName = "aChronic ", menuName = "Augments/aChronic")]
public class aChronic : AugmentData
{
    float chronicBonus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        chronicBonus = .2f + (.1f * currentLevel);
        playerCombat.statusDurationMultiplier = 1f + chronicBonus;
    }
}