using UnityEngine;
using System.Collections;

public class Chakram : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private GameObject currentTarget;
    private float speed = 20f;
    private int maxChakramBounces = 6;
    private int currentChakramBounces = 0;
    private bool canHit = true;
    private float attackSpeed = 2f;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        currentTarget = playerCombat.closestCurrentEnemy;
    }

    void Update()
    {
        transform.Rotate(0, 0, 1500f * Time.deltaTime);
        if (currentTarget == null)
        {
            FindNextTarget(null);
            if (currentTarget == null)
            {
                Destroy(gameObject);
                return;
            }
        }
        if (currentChakramBounces >= maxChakramBounces)
        {
            Destroy(gameObject);
            return;
        }

        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && canHit)
        {
            canHit = false;

            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.reduceHp(20f);

            currentChakramBounces++;

            if (currentChakramBounces < maxChakramBounces)
            {
                FindNextTarget(collision.gameObject);
            }

            StartCoroutine(HitCooldown());
        }
    }

    void FindNextTarget(GameObject lastHit)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy != lastHit)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        if (closestEnemy == null)
        {
            currentChakramBounces = maxChakramBounces; 
        }

        currentTarget = closestEnemy;
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canHit = true;
    }
}