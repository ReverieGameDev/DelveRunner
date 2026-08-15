using UnityEngine;

public class SoulCoinPickup : MonoBehaviour
{
    private bool soulCoinTowardsPlayer = false;
    private int xpSpeed = 14;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (soulCoinTowardsPlayer == true)
        {
            transform.Translate((PlayerCombat.Instance.transform.position - transform.position).normalized * xpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, PlayerCombat.Instance.transform.position) < 0.5f)
            {
                PlayerCombat.Instance.soulCoins++; 
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            soulCoinTowardsPlayer = true;
        }
    }
}
