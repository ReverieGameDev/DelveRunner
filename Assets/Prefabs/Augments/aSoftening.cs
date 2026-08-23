using UnityEngine;
[CreateAssetMenu(fileName = "aSoftening", menuName = "Augments/aSoftening")]
public class aSoftening : AugmentData
{
    int currentAugmentLevel;
    int enfeebleChance;
    int bonusEnfeebleDamage;
    int enfeebleDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
        currentAugmentLevel = currentLevel;
        enfeebleChance = 5 + (10 * currentAugmentLevel);
        bonusEnfeebleDamage = 5 + (5 * currentAugmentLevel);
        enfeebleDuration = 3 + (currentAugmentLevel);
    }
    private void OnHit(Enemy enemy)
    {
        if (Random.Range(1, 101) <= enfeebleChance)
        {
            enemy.GetComponent<EnemyStatusEffects>().ESEEnfeeble(enfeebleDuration, bonusEnfeebleDamage);
        }
    }
}