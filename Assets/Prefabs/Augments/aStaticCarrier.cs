using UnityEngine;

[CreateAssetMenu(fileName = "aStaticCarrier", menuName = "Augments/aStaticCarrier")]
public class aStaticCarrier : AugmentData
{
    int aShockChance;
    int currentStaticLevel;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.aStaticCarrierActive = true;
        currentStaticLevel = currentLevel;
        playerCombat.aStaticCarrierChance = (15 * currentLevel) + 10;
        aShockChance = (10 * currentLevel) + 10;
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
    }
    private void OnHit(Enemy enemy)
    {
        if (Random.Range(0,101) <= aShockChance)
        {
            enemy.GetComponent<EnemyStatusEffects>().ESEShock((3 + currentStaticLevel) * 2, currentStaticLevel + 1, 3, 3);
        }
    }
}