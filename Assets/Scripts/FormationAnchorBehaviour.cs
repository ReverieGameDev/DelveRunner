using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FormationAnchorBehaviour : MonoBehaviour
{
    private float speed = 4f;
    private Vector2 chaseDirection;
    private Vector2 retreatDirection;
    private Transform player;
    private List<WarriorSkeleton> warriorsList = new List<WarriorSkeleton>();
    private float minAngle;
    private float maxAngle;
    private PlayerMovement playerMovement;
    public float angleOffset = 10f;
    public bool chargeAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        StartCoroutine("FindAllWarriors");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance((Vector2)transform.position, player.position) >= 16)
        {
            chaseDirection = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
            transform.position = (Vector2)transform.position + chaseDirection;
        }

        else if (Vector2.Distance((Vector2)transform.position, player.position) <= 7)
        {
            retreatDirection = new Vector2(transform.position.x - player.position.x, transform.position.y - player.position.y).normalized*2;
            transform.position = (Vector2)transform.position + retreatDirection;
        }
        FindMinMaxAngle();
    }

    IEnumerator FindAllWarriors()
    {
        yield return new WaitForSeconds(.25f);
        foreach (WarriorSkeleton warrior in FindObjectsByType<WarriorSkeleton>(FindObjectsSortMode.None))
        {
            if(Vector2.Distance(transform.position, warrior.transform.position) < 5)
            {
                warriorsList.Add(warrior);
            }
        }
    }

    private void FindMinMaxAngle()
    {
        if (warriorsList.Count == 0) return;
        WarriorSkeleton minAngleScript = warriorsList[0];
        WarriorSkeleton maxAngleScript = warriorsList[0];
        
        for (int i = 0; i < warriorsList.Count-1; i++)
        {
            if (Mathf.Rad2Deg*(Mathf.Atan2(transform.position.y - minAngleScript.transform.position.y, transform.position.x - minAngleScript.transform.position.x)) > Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - warriorsList[i+1].transform.position.y, transform.position.x -warriorsList[i+1].transform.position.x)))
            {
                minAngleScript = warriorsList[i + 1];
            }
            if (Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - maxAngleScript.transform.position.y, transform.position.x - maxAngleScript.transform.position.x)) < Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - warriorsList[i + 1].transform.position.y, transform.position.x - warriorsList[i + 1].transform.position.x)))
            {
                maxAngleScript = warriorsList[i + 1];
            }
        }
        minAngle = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - minAngleScript.transform.position.y, transform.position.x - minAngleScript.transform.position.x));
        maxAngle = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - maxAngleScript.transform.position.y, transform.position.x - maxAngleScript.transform.position.x));

        float playerAngle = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - playerMovement.transform.position.y, transform.position.x - playerMovement.transform.position.x));
        
        if (playerAngle < minAngle - angleOffset || playerAngle > maxAngle + angleOffset)
        {
            chargeAttack = true;
        }
    }
}

