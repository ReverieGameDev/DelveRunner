using UnityEngine;
[CreateAssetMenu(fileName = "aScholar", menuName = "Augments/aScholar")]
public class aScholar : AugmentData
{
    int scholarXP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        scholarXP = currentLevel;
        playerCombat.scholarXPAmount = scholarXP;
        playerCombat.scholarActive = true;
    }
}