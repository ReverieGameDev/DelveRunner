using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ShatterFrost : MonoBehaviour
{
    private List<Enemy> frozenEnemies = new List<Enemy>();
    private PlayerCombat playerCombat;
    private float freezeDuration = 2f;
    public bool canFreeze = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        GetComponent<Animator>().SetBool("ShatterFrostParam", true);
        canFreeze = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCombat.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && canFreeze)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (!frozenEnemies.Contains(enemy) && !enemy.bossMonster)
            {
                frozenEnemies.Add(collision.GetComponent<Enemy>());
                
                StartCoroutine("FreezeEnemy", enemy);
            }

        }
    }
    IEnumerator FreezeEnemy(Enemy enemy)
    {
        yield return new WaitForSeconds(.1f);
        canFreeze = false;
        enemy.enemySpeed /= 4;
        enemy.reduceHp(15f);
        enemy.GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(freezeDuration);
        enemy.GetComponent<SpriteRenderer>().color = Color.white;
        enemy.enemySpeed *= 4;
        if (frozenEnemies[frozenEnemies.Count - 1] == enemy)
        {
            Destroy(gameObject);
        }
    }
}
