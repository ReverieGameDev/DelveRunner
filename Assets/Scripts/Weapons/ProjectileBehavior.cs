using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private PlayerCombat playerCombat;
    Vector3 bulletTrajectory;
    public float bulletSpeed = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        bulletTrajectory = playerCombat.closestCurrentEnemy.transform.position - playerCombat.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((bulletTrajectory.normalized) * Time.deltaTime * bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyHit = collision.GetComponent<Enemy>();
            enemyHit.reduceHp(playerCombat.attack);
            Destroy(gameObject);
        }
    }
}
