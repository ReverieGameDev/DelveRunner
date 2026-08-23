using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "aAfterburn", menuName = "Augments/aAfterburn")]
public class aAfterburn : AugmentData
{
    int afterburnCurrentLevel;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnEnemyKill -= OnKill;
        playerCombat.OnEnemyKill += OnKill;
        afterburnCurrentLevel = currentLevel;
    }
    private void OnKill(Enemy enemy)
    {
        if (enemy.GetComponent<EnemyStatusEffects>().activeStatusEffects.Any(effect => effect.type == WeaponStatusEffect.Burn))
        {
            Instantiate(PlayerCombat.Instance.emberGainPrefab, PlayerCombat.Instance.transform.position,Quaternion.identity);
            EmberSystem.Instance.AddEmber(afterburnCurrentLevel * 5);
        }
    }
}