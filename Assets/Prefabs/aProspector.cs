using UnityEngine;
[CreateAssetMenu(fileName = "aProspector", menuName = "Augments/aProspector")]
public class aProspector : AugmentData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aProspectorActive = true;
        playerCombat.aProspectorChance = (15 * (currentLevel + 1))-5;
    }
}