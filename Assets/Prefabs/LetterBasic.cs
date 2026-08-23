using System.Collections.Generic;
using UnityEngine;

public class LetterBasic : BasicAttackBase
{
    public float damageMultiplier = 1f;
    private WeaponData wepData;
    private Vector2 projectileDirection;
    public bool bloodletterActive = false;
    public int bloodletterChance = 40;
    public GameObject bloodletterPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void FireBaseAttack(WeaponData weaponData, Vector2 direction)
    {
        wepData = weaponData;
        projectileDirection = direction;
    }
    void Start()
    {
        transform.position = new Vector3(transform.position.x + projectileDirection.x, transform.position.y + projectileDirection.y);
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector3(1, -2, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        Enemy enemyScript = collision.GetComponent<Enemy>();
        PlayerCombat.Instance.DealDamage(enemyScript, wepData.wDamage * damageMultiplier);
        if (bloodletterActive && Random.Range(1, 101) < bloodletterChance)
        {
            collision.GetComponent<EnemyStatusEffects>().ESEBleed(4f, 4, 1f);
            Instantiate(bloodletterPrefab,collision.transform.position, Quaternion.identity);
        }
    }

    public void DestroyLetter()
    {
        Destroy(gameObject);
    }
}
