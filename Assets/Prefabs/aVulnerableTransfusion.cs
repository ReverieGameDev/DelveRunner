using UnityEngine;
[CreateAssetMenu(fileName = "aVulnerableTransfusion", menuName = "Augments/VulnerableTransfusion", order = 1)]
public class aVulnerableTransfusion : AugmentData
{
    float threshold = .45f;
    int augmentCurrentLevel;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt += OnHit;
        augmentCurrentLevel = currentLevel;
    }
    private void OnHit(Enemy enemy)
    {
        if (enemy.enemyHealth/ enemy.enemyData.health  < threshold)
        {
            PlayerCombat.Instance.BloodHeal(5);
        }
    }
}