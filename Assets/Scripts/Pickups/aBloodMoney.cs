using UnityEngine;
[CreateAssetMenu(fileName = "aBloodMoney", menuName = "Augments/aBloodMoney")]
public class aBloodMoney : AugmentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aBloodMoneyActive = true;
        playerCombat.aBloodMoneyHeal = currentLevel;
    }
}