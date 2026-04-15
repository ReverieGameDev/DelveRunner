using System.Collections;
using UnityEngine;

public class HealthBottleBehaviour : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool hpTowardsPlayer = false;
    private int hpSpeed = 10;
    private float healFactor = 20;
    public string typeOfHeal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        switch (typeOfHeal)
        {
            case "small":
                healFactor = 15;
                break;
            case "medium":
                healFactor = 25;
                break;
            case "large":
                healFactor = 40;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hpTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * hpSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
            {
                playerCombat.HealPlayer(healFactor);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            hpTowardsPlayer = true;

        }
    }
}
