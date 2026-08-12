using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class StarCallerBasic : BasicAttackBase
{
    private WeaponData wepData;
    private Vector2 projectileDirection;
    public bool starMasher = false;
    public float damageMultiplier = 1f;
    private EmberSystem emberSystem;
    public GameObject starcallerEmberProcPrefab;
    private Vector2 startingPos;
    private Rigidbody2D rb;
    public override void FireBaseAttack(WeaponData weaponData, Vector2 direction)
    {
        wepData = weaponData;
        projectileDirection = direction;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        transform.position = new Vector3(transform.position.x + projectileDirection.x, transform.position.y + projectileDirection.y);
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        emberSystem = FindFirstObjectByType<EmberSystem>();
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector3(1, -1, 1);
        rb.linearVelocity = projectileDirection * wepData.wProjectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(startingPos, transform.position) > wepData.wRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        Enemy enemyScript = collision.GetComponent<Enemy>();
        int dmg = PlayerCombat.Instance.CalcWeaponDamage(wepData.wDamage * damageMultiplier, out bool wasCrit);
        enemyScript.reduceHp(dmg, 1, wasCrit);
        if (UnityEngine.Random.Range(0, 100) < wepData.wProcChance)
        {
            emberSystem.AddEmber(5);
            Instantiate(starcallerEmberProcPrefab, PlayerCombat.Instance.transform, false);
        }
        
        Destroy(gameObject);
    }
}
