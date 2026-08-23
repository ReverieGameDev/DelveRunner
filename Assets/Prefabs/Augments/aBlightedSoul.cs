using UnityEngine;
[CreateAssetMenu(fileName = "aBlightedSoul", menuName = "Augments/aBlightedSoul")]
public class aBlightedSoul : AugmentData
{
    int currentBlightedLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
        playerCombat.blightedSoulActive = true;
        currentBlightedLevel = currentLevel;
    }
    private void OnHit(Enemy enemy)
    {
        enemy.GetComponent<EnemyStatusEffects>().ESEPoison(4 + (currentBlightedLevel), currentBlightedLevel, 1);
    }
}