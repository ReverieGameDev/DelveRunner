using System.Collections.Generic;
using UnityEngine;

public class VerdantMawCharge : ChargeAttackBase
{
    public float damageMultiplier = 1f;
    private List<Enemy> enemiesHit = new List<Enemy>();
    private WeaponData wepData;
    private float currentCharge;
    private float maxCharge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Fire(float secondsHeld, float secondsToMaxCharge)   // <-- the required method, on THIS class
    {
        currentCharge = secondsHeld;
        maxCharge = secondsToMaxCharge;
    }
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        Enemy enemyScript = collision.GetComponent<Enemy>();
        if (!enemiesHit.Contains(enemyScript))
        {
            enemiesHit.Add(enemyScript);
            if (Random.Range(0, 100) < wepData.wChargeProcChance)
            {
                if (enemyScript.TryGetComponent<EnemyStatusEffects>(out EnemyStatusEffects enemyStatusEffects))
                {
                    enemyStatusEffects.ESEPoison(wepData.wChargeEffectDuration, (int)(wepData.wChargeEffectDamage * (currentCharge/maxCharge)), wepData.wChargeEffectTickRate);
                }
            }
        }
    }

    public void DestroyVerdantMaw()
    {
        Destroy(gameObject);
    }
}
