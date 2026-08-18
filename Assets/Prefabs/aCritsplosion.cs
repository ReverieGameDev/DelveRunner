using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "aCritsplosion", menuName = "Augments/aCritsplosion")]
public class aCritsplosion : AugmentData
{
    int aCritsplosionCurrentLevel;

    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnEnemyKill -= OnKill;
        playerCombat.OnEnemyKill += OnKill;
        aCritsplosionCurrentLevel = currentLevel;
    }

    private void OnKill(Enemy enemy)
    {
        if (!enemy.diedToCrit) return;

        float radius = aCritsplosionCurrentLevel * 2 + 3;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = enemies
            .Where(inRange => inRange != enemy.gameObject
                && Vector2.Distance(inRange.transform.position, enemy.transform.position) <= radius)
            .ToList();

        List<ActiveStatusEffects> statusSpreader =
            enemy.GetComponent<EnemyStatusEffects>().activeStatusEffects.ToList();

        foreach (GameObject target in enemiesInRange)
        {
            EnemyStatusEffects ese = target.GetComponent<EnemyStatusEffects>();
            if (ese == null) continue;

            foreach (ActiveStatusEffects effect in statusSpreader)
            {
                switch (effect.type)
                {
                    case WeaponStatusEffect.Burn:
                        ese.ESEBurn(effect.duration * 0.5f, effect.damage, effect.tickRate);
                        break;
                    case WeaponStatusEffect.Poison:
                        ese.ESEPoison(effect.duration * 0.5f, effect.damage, effect.tickRate);
                        break;
                    case WeaponStatusEffect.Enfeeble:
                        ese.ESEEnfeeble(effect.duration * 0.5f, effect.effectPercentage);
                        break;
                }
            }
        }

        Instantiate(PlayerCombat.Instance.critSplosionPrefab, enemy.transform.position, Quaternion.identity);
    }
}