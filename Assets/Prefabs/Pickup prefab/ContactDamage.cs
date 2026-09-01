using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int enemyDamage = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerCombat.Instance.DamagePlayer(enemyDamage);
        }
    }
}
