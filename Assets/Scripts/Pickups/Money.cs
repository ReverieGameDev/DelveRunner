using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool moneyTowardsPlayer = false;
    private int moneySpeed = 6;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * moneySpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            
            moneyTowardsPlayer = true;

        }
    }
}
