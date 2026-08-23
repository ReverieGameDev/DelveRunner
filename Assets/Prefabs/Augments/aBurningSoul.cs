using UnityEngine;
[CreateAssetMenu(fileName = "aBurningSoul", menuName = "Augments/aBurningSoul")]
public class aBurningSoul : AugmentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.burningSoulActive = true;
        playerCombat.burningSoulLevel = currentLevel;
        playerCombat.burningSoulMaxDamage = 1.25f + (0.25f * currentLevel);
    }
}