using UnityEngine;

public class GlitchSwordAttack : MonoBehaviour
{
    private int enemiesHit;
    private Enemy enemy;
    private float glitchSwordDamage = 30f;
    private float glitchSwordSpeed = 20f;
    private int maxEnemiesHit = 1;
    private AttackManager attackManager;
    private Vector3 trajectory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackManager = FindFirstObjectByType<AttackManager>();
        trajectory = attackManager.mousePos - attackManager.playerPos;

        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((trajectory.normalized) * Time.deltaTime * glitchSwordSpeed, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.reduceHp(glitchSwordDamage);
            enemiesHit++;
            if (enemiesHit >= maxEnemiesHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
