using System.Collections;
using UnityEngine;

public class DeathBossScript : MonoBehaviour
{
    private WaveManager waveManager;
    // Components
    private Animator animator;
    private PlayerMovement playerMovement;
    private ShopManager shopManager;

    // Prefabs
    public GameObject deathSlashAttackPrefab;
    public GameObject deathSummon;
    public GameObject teleportCircle;

    // State
    private bool isDead = false;

    void Start()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
        waveManager = FindFirstObjectByType<WaveManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        animator = GetComponent<Animator>();
        StartCoroutine(RandomAttack());
    }

    IEnumerator RandomAttack()
    {
        if (isDead) yield break;

        yield return new WaitForSeconds(0.2f);

        int randomAttack = Random.Range(0, 3);

        if (randomAttack == 0)
            yield return StartCoroutine(DeathSlashAttack());
        else if (randomAttack == 1)
            yield return StartCoroutine(DeathTeleportAttack());
        else if (randomAttack == 2)
            yield return StartCoroutine(DeathSummonAttack());

        StartCoroutine(RandomAttack());
    }

    IEnumerator DeathSlashAttack()
    {
        if (isDead) yield break;

        animator.SetInteger("DeathAttack", 1);
        yield return new WaitForSeconds(0.25f);

        Instantiate(deathSlashAttackPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);

        Instantiate(deathSlashAttackPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        animator.SetInteger("DeathAttack", 0);
    }

    IEnumerator DeathSummonAttack()
    {
        if (isDead) yield break;

        animator.SetInteger("DeathAttack", 2);

        for (int i = 0; i < 4; i++)
        {
            if (isDead) yield break;

            float randomX = Random.Range(playerMovement.transform.position.x - 50, playerMovement.transform.position.x + 50);
            float randomY = Random.Range(playerMovement.transform.position.y - 50, playerMovement.transform.position.y + 50);
            Vector3 randomPos = new Vector3(randomX, randomY);

            Instantiate(deathSummon, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
        }

        animator.SetInteger("DeathAttack", 0);
    }

    IEnumerator DeathTeleportAttack()
    {
        if (isDead) yield break;

        yield return new WaitForSeconds(2.5f);

        animator.SetInteger("DeathAttack", 3);
        Vector3 teleportPos = playerMovement.transform.position;

        Instantiate(teleportCircle, teleportPos, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        transform.position = teleportPos;
        animator.SetInteger("DeathAttack", 0);
    }

    /*public IEnumerator DeathBossDeath()
    {
        isDead = true;
        animator.SetInteger("DeathAttack", 4);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        waveManager.bossWave = false;
        waveManager.Waves();
        shopManager.OpenShop();
    }*/
}