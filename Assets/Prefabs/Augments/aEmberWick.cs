using UnityEngine;
[CreateAssetMenu(fileName = "aEmberWick", menuName = "Augments/aEmberWick")]
public class aEmberWick: AugmentData
{
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aEmberWickActive = true;
        playerCombat.aEmberWickStatusExtend = (1 + ((.2f * currentLevel) +.1f));
    }
}