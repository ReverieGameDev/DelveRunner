using System.Collections;
using UnityEngine;

public class MissleAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private float missleSpeed = 10f;
    private Vector2 missleToPlayerAngle;
    private float missleAngle;
    private float missleDuration = 4f;
    public bool isReadyToDamage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + missleToPlayerAngle * missleSpeed * Time.fixedDeltaTime);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isReadyToDamage)
        {
            PlayerCombat.Instance.DamagePlayer(10f);
        }
    }

    public void EnableDamage()
    {
        isReadyToDamage = true;
    }
    public void DestroyMagicMissile()
    {
        Destroy(gameObject);
    }
}
