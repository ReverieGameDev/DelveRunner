using UnityEngine;
[CreateAssetMenu(fileName = "aRekindle", menuName = "Augments/aRekindle")]
public class aRekindle : AugmentData
{
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.rekindleEmberPerKill = currentLevel;
        playerCombat.isRekindleActive = true;
    }
}