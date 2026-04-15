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
    public int backlineEnemiesLeftAlive;
    public bool canWarriorLeap = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DestroyAnchor()
    {
        Destroy(gameObject);
    }
}

/*
if (Vector2.Distance((Vector2)transform.position, player.position) >= 25)
{
    chaseDirection = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
    transform.position = (Vector2)transform.position + chaseDirection;
}

else if (Vector2.Distance((Vector2)transform.position, player.position) <= 6)
{
    retreatDirection = new Vector2(transform.position.x - player.position.x, transform.position.y - player.position.y).normalized*2;
    transform.position = (Vector2)transform.position + retreatDirection;
}*/