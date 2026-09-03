using UnityEngine;

public class ETNullCircle : MonoBehaviour
{
    private float tickTimer;
    private bool playerInNullCircle = false;
    public float tickInterval = 4f;
    public float nullDamage = 6f;
    void Update()
    {
        if (playerInNullCircle)
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                PlayerCombat.Instance.EnvironmentDamagePlayer(nullDamage);
                tickTimer = tickInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInNullCircle = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInNullCircle = false;
        }
    }
}
