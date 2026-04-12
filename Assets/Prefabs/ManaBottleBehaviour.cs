using System.Collections;
using UnityEngine;
public class ManaBottleBehaviour : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private bool manaTowardsPlayer = false;
    private int manaSpeed = 10;
    private float manaFactor = 20;
    public string typeOfMana;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        switch (typeOfMana)
        {
            case "small":
                manaFactor = 20;
                break;
            case "medium":
                manaFactor = 40;
                break;
            case "large":
                manaFactor = 60;
                break;
        }
    }

    void Update()
    {
        if (manaTowardsPlayer == true)
        {
            transform.Translate((playerCombat.transform.position - transform.position).normalized * manaSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 0.5f)
            {
                playerCombat.currentPlayerMana = Mathf.Min(playerCombat.currentPlayerMana + manaFactor, playerCombat.playerManaBase);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manaTowardsPlayer = true;
        }
    }
}