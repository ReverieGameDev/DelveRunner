using UnityEngine;

public class TwinShadowsAttack : MonoBehaviour
{
    private int enemiesHit;
    private Enemy enemy;
    private float glitchSwordDamage = 30f;
    private float glitchSwordSpeed = 20f;
    private int maxEnemiesHit = 1;
    private AttackManager attackManager;
    private Vector3 trajectory;
    private PlayerCombat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        attackManager = FindFirstObjectByType<AttackManager>();
        trajectory = attackManager.mousePos - attackManager.playerPos;

        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerCombat.transform.position) > Mathf.Abs(30))
        {
            Destroy(gameObject);
        }
        transform.Translate((trajectory.normalized) * Time.deltaTime * glitchSwordSpeed, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.reduceHp(playerCombat.CalcWeaponDamage(glitchSwordDamage));
            enemiesHit++;
            if (enemiesHit >= maxEnemiesHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
