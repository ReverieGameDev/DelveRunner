using System.Collections;
using UnityEngine;

public class WarriorSlash : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("DestroyAfterAnimationPlaceholder");
    }

    IEnumerator DestroyAfterAnimationPlaceholder()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat.Instance.DamagePlayer(15f);
        }
    }
}
