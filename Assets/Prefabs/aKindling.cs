using UnityEngine;
[CreateAssetMenu(fileName = "aKindling", menuName = "Augments/Kindling", order = 1)]
public class aKindling : AugmentData
{
    int kindlingCounter = 4;
    int currentHitsCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
    }
    private void OnHit(Enemy enemy)
    {
        currentHitsCounter++;
        if (currentHitsCounter >= kindlingCounter)
        {
            enemy.GetComponent<EnemyStatusEffects>().ESEBurn(4, 3, 1);
            currentHitsCounter = 0;
        }
        Debug.Log("kindling heard a hit");
    }
}