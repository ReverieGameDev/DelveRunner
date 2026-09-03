using UnityEngine;
[CreateAssetMenu(fileName = "aPoisonDippedDagger", menuName = "Augments/aPoisonDippedDagger")]
public class aPoisonDippedDagger : AugmentData
{
    int currentAugmentLevel;
    int poisonChance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
        currentAugmentLevel = currentLevel;
        poisonChance = 5 + (10 * currentAugmentLevel);
    }
    private void OnHit(Enemy enemy)
    {
        if (Random.Range(1, 101) <= poisonChance)
        {
            
            enemy.GetComponent<EnemyStatusEffects>().ESEPoison(4+ currentAugmentLevel, currentAugmentLevel, 1);
        }
    }
}