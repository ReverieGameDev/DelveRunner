/*using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections;

public class BlackHoleAttack : MonoBehaviour
{
    private Enemy enemy;
    private PlayerCombat playerCombat;
    private float blackHolePullStrength = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.reduceHp(.1f);
            enemy.transform.Translate(((transform.position - enemy.transform.position).normalized) * blackHolePullStrength * Time.deltaTime);
        }
    }


}*/
