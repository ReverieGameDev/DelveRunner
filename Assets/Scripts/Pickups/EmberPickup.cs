using UnityEngine;

public class EmberPickup : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool emberTowardsPlayer = false;
    private int xpSpeed = 14;
    private EmberSystem emberSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emberSystem = FindFirstObjectByType<EmberSystem>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (emberTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * xpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
            {

                emberSystem.AddEmber(40); // Call the method directly, not the string name
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            emberTowardsPlayer = true;
        }
    }
}
