using UnityEngine;

public class Xp : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool xpTowardsPlayer = false;
    private int xpSpeed = 14;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xpTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position-transform.position).normalized * xpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
            {
                Destroy(gameObject);
                playerCombat.playerXp += 10;
                playerCombat.addExp();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            xpTowardsPlayer = true;
            
        }
    }
}
