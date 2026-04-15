using UnityEngine;

public class SkeletonCat : MonoBehaviour
{
    private Enemy enemy;
    private Vector2 enemyPosition;
    private PlayerMovement playerMovement;
    private bool isDead = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        anim.SetInteger("CatInt", 0);
    }

    void FixedUpdate()
    {
        if (enemy.enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            CatDeathSequence();
        }
        else if (enemy.enemyHealth > 0 && !isDead)
        {
            float enemySpeed = enemy.enemySpeed;
            enemyPosition = transform.position;
            transform.Translate(((playerMovement.playerPosition - enemyPosition).normalized) * enemySpeed * Time.fixedDeltaTime);
        }

    }

    private void CatDeathSequence()
    {
        anim.SetInteger("CatInt", 1);
        GetComponent<Collider2D>().enabled = false;
    }
    public void DestroyCat()
    {
        Destroy(gameObject);
    }
}
