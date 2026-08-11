
using System.Collections.Generic;
using UnityEngine;

public class VerdantMawBasic : BasicAttackBase
{
    private PlayerCombat playerCombat;
    public float damageMultiplier = 1f;
    private List<Enemy> enemiesHit = new List<Enemy>();
    private WeaponData wepData;
    private Vector2 projectileDirection;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void WeaponInit(WeaponData weaponData, Vector2 direction)
    {
        wepData = weaponData;
        projectileDirection = direction;
    }
    void Start()
    {
        transform.position = new Vector3(transform.position.x + projectileDirection.x, transform.position.y + projectileDirection.y);
        rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector3(1, -1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        Enemy enemyScript = collision.GetComponent<Enemy>();
        if (!enemiesHit.Contains(enemyScript))
        {
            enemiesHit.Add(enemyScript);
            int dmg = PlayerCombat.Instance.CalcWeaponDamage(wepData.wDamage * damageMultiplier, out bool wasCrit);
            enemyScript.reduceHp(dmg, 1, wasCrit);
            if (Random.Range(0, 100) < wepData.wProcChance)
            {
                if (enemyScript.TryGetComponent<EnemyStatusEffects>(out EnemyStatusEffects enemyStatusEffects))
                {
                    enemyStatusEffects.ESEPoison(wepData.wChargeEffectDuration, wepData.wStatusEffectDamage,wepData.wStatusEffectTickRate);
                }
            }
        }
    }

    public void DestroyVerdantMaw()
    {
        Destroy(gameObject);
    }
}
