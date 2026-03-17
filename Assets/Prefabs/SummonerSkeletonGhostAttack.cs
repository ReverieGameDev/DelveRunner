using System.Collections;
using UnityEngine;

public class SummonerSkeletonGhostAttack : MonoBehaviour
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
        StartCoroutine("MissileTimer");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        missleToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
        missleAngle = Mathf.Rad2Deg * (Mathf.Atan2(missleToPlayerAngle.y, missleToPlayerAngle.x));
        transform.rotation = Quaternion.Euler(0, 0, missleAngle);
        rb.MovePosition((Vector2)transform.position + missleToPlayerAngle * missleSpeed * Time.fixedDeltaTime);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat.Instance.DamagePlayer(10f);
            DestroyMagicMissile();
        }
    }

    IEnumerator MissileTimer()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    public void DestroyMagicMissile()
    {
        Destroy(gameObject);
    }
}