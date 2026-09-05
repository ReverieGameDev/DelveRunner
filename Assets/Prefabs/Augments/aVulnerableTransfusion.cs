using UnityEngine;
[CreateAssetMenu(fileName = "aVulnerableTransfusion", menuName = "Augments/VulnerableTransfusion", order = 1)]
public class aVulnerableTransfusion : AugmentData
{
    private float threshold => 0.10f * augmentCurrentLevel;
    int augmentCurrentLevel;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt += OnHit;
        augmentCurrentLevel = currentLevel;
    }
    private void OnHit(Enemy enemy)
    {
        if (enemy.enemyHealth / enemy.maxEnemyHealth < threshold)
        {
            PlayerCombat.Instance.BloodHeal(1);
        }
    }
}