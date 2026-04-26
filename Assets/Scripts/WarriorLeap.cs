using System.Collections;
using UnityEngine;

public class WarriorLeap : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public TankSkeleton owner;
    private PlayerStatusEffects playerStatusEffects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatusEffects = FindFirstObjectByType<PlayerStatusEffects>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        StartCoroutine("DestroyHitbox");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCombat.DamagePlayer(owner.leapDamage);
            playerStatusEffects.ApplyStatus("stun", 1f,0);
        }
    }
    IEnumerator DestroyHitbox()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
