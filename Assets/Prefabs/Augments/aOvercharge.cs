using UnityEngine;

[CreateAssetMenu(fileName = "aOvercharge", menuName = "Augments/aOvercharge")]
public class aOvercharge : AugmentData
{
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aOverchargeActive = true;
    }
}