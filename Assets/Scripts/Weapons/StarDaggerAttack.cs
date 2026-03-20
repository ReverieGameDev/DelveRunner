using UnityEngine;

public class StarDaggerAttack : MonoBehaviour
{
    private Enemy enemy;
    private float starDaggerDamage = 30f;
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
        transform.rotation = Quaternion.Euler(0, 0, angle+90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCombat.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.reduceHp(playerCombat.CalcWeaponDamage(starDaggerDamage));
        }
    }

    public void DestroyStarDagger()
    {
        Destroy(gameObject);
    }
}
