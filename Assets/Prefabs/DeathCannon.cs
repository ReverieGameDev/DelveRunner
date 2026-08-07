using UnityEngine;

public class DeathCannon : ProjectileBase

{

    private AttackManager attackManager;
    private Vector3 trajectory;
    public float damageMultiplier = 1f;
    private Vector2 destination;
    private Rigidbody2D rb;
    private WeaponData wepData;
    private Vector2 projectileDirection;
    private Vector2 startingPos;
    private bool explode = false;
    public float explosionRadius = 3f;
    public GameObject DeathCannonExplosionPrefab;
    public int explosionDamage = 10;
    private Enemy enemyFirstHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void WeaponInit(WeaponData weaponData, Vector2 direction)
    {
        wepData = weaponData;
        projectileDirection = direction;
    }
    void Start()
    {
        transform.position = new Vector3(transform.position.x + projectileDirection.x * 7, transform.position.y + projectileDirection.y * 7);
        startingPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.linearVelocity = projectileDirection * wepData.wProjectileSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (!explode && Vector2.Distance(startingPos, transform.position) > wepData.wRange)
        {
            explode = true;
            Explode();
        }
    }
    private void Explode()
    {
        Instantiate (DeathCannonExplosionPrefab, transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy") || hit.GetComponent<Enemy>() == enemyFirstHit) continue;
            Enemy e = hit.GetComponent<Enemy>();
            int dmg = PlayerCombat.Instance.CalcWeaponDamage(explosionDamage * damageMultiplier, out bool wasCrit);
            e.reduceHp(dmg, 1, wasCrit);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (explode) return;
            enemyFirstHit = collision.GetComponent<Enemy>();
            int dmg = PlayerCombat.Instance.CalcWeaponDamage(wepData.wDamage * damageMultiplier, out bool wasCrit);
            enemyFirstHit.reduceHp(dmg, 1, wasCrit);
            explode = true;
            Explode();
        }
    }
}
