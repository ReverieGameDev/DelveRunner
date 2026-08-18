using UnityEngine;
[CreateAssetMenu(fileName = "aCullTheMeek", menuName = "Augments/aCullTheMeek")]
public class aCullTheMeek : AugmentData
{
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.cullTheMeekActive = true;
        playerCombat.cullTheMeekBonusDmg = 1.05f + (0.1f*currentLevel);
    }
}