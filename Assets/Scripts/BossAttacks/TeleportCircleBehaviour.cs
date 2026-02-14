using UnityEngine;
using System.Collections;
public class TeleportCircleBehaviour : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private int explosionCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        explosionCounter++;
        if (collision.CompareTag("Player") && explosionCounter>1)
        {
            Debug.Log("collider deteceted");
            StartCoroutine("TeleportCircleExplode");
        }
    }

    IEnumerator TeleportCircleExplode()
    {
        Debug.Log("teleport circle coroutine");
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.6f);
        if (playerCombat.iFrames == false)
        {
            playerCombat.DamagePlayer();
        }
        Destroy(gameObject);
    }
}
