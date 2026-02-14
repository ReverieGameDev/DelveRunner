using System.Collections;
using UnityEngine;

public class FantasiaAttack : MonoBehaviour
{
    private Enemy[] enemies;
    private SpriteRenderer sr;
    private Color c;
    private PlayerCombat playerCombat;
    public GameObject galaxyExplosion;
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        sr = GetComponent<SpriteRenderer>();
        c = sr.color;
        c.a = 0f;
        sr.color = c;
        StartCoroutine(GalaxyAnimation());
    }
    void Update()
    {
        transform.position = playerCombat.transform.position;
    }
    void findAllEnemies()
    {
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemyToBeDestroyed in enemies)
        {
            if (!enemyToBeDestroyed.bossMonster && !enemyToBeDestroyed.isMinion)
            {
                Instantiate(galaxyExplosion, enemyToBeDestroyed.transform.position, Quaternion.identity);
                enemyToBeDestroyed.reduceHp(1000000f);
            }
            else
            {
                enemyToBeDestroyed.reduceHp(100f);
            }
        }
    }

    IEnumerator GalaxyAnimation()
    {
        for (int i = 0; i < 200; i++)
        {
            transform.Rotate(0, 0, .1225f);
            c.a += 0.005f;
            sr.color = c;
            yield return new WaitForSeconds(.025f);
        }
        findAllEnemies();
        Destroy(gameObject);
    }
}