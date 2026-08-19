using UnityEngine;
[CreateAssetMenu(fileName = "aStarCallerNova", menuName = "Augments/aStarCallerNova")]
public class aStarCallerNova : AugmentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aStarCallerNovaActive = true;
        playerCombat.aStarCallerNovaDamageMult = (1.5f*currentLevel) + 1;
        playerCombat.aStarCallerNovaCinderChance = currentLevel * 20 + 10;
        playerCombat.aStarCallerNovaEmberCost = 10;
    }
}