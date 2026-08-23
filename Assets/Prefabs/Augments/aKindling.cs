using UnityEngine;
[CreateAssetMenu(fileName = "aKindling", menuName = "Augments/Kindling", order = 1)]
public class aKindling : AugmentData
{
    int kindlingCounter = 4;
    int currentHitsCounter;
    int currentKindlingLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
        currentKindlingLevel = currentLevel;
        kindlingCounter = 6-currentLevel;
    }
    private void OnHit(Enemy enemy)
    {
        currentHitsCounter++;
        if (currentHitsCounter >= kindlingCounter)
        {
            enemy.GetComponent<EnemyStatusEffects>().ESECinder(3+currentKindlingLevel, 2+2*currentKindlingLevel,1, currentKindlingLevel);
            currentHitsCounter = 0;
        }
    }
}