using UnityEngine;

public class StarDaggerAttack : MonoBehaviour
{
    private Enemy enemy;
    private float starDaggerDamage = 12f;
    private AttackManager attackManager;
    private Vector3 trajectory;
    private PlayerCombat playerCombat;
    public float damageMultiplier = 1f;
    private EmberSystem emberSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emberSystem = FindFirstObjectByType<EmberSystem>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        attackManager = FindFirstObjectByType<AttackManager>();
        trajectory = attackManager.mousePos - attackManager.playerPos;
        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        DoSwipeHit();   // capture the hit at the instant of firing
    }
    private void DoSwipeHit()
    {
        float radius = 4.5f;          // swipe reach
        float halfArc = 90f;        // 120° cone total
        Vector2 aimDir = trajectory.normalized;

        // BROAD PHASE: everyone in range, right now
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerCombat.transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;
            // NARROW PHASE: only who's inside the arc
            Vector2 dirToEnemy = (hit.transform.position - playerCombat.transform.position).normalized;
            if (Vector2.Angle(aimDir, dirToEnemy) > halfArc) continue;

            // APPLY
            Enemy e = hit.GetComponent<Enemy>();
            int dmg = playerCombat.CalcWeaponDamage(starDaggerDamage * damageMultiplier, out bool wasCrit);
            e.reduceHp(dmg,1, wasCrit);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void DestroyStarDagger()
    {
        Destroy(gameObject);
    }
}
