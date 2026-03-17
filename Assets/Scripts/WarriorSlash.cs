using System.Collections;
using UnityEngine;

public class WarriorSlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
