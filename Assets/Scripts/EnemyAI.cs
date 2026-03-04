using System.Collections;
using System.Data;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState;
    private Transform player;
    private Rigidbody2D rb;
    private float speed = 12f;
    private SpawnManager spawnManager;
    private Vector2 centerFormationTile;
    public GameObject assignedSpawnAnchor;
    Vector2 anchorPlayerAngle;
    Vector2 guardPos;
    Vector2 directionToGuard;
    Vector2 retreatPos;
    Vector2 directionToRetreat;
    Vector2 retreatStartPos;
    private float retreatSpeed = .5f;
    private bool hasStartedRetreating = false;
    public EnemyRoles role;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        currentState = EnemyState.Chase;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        centerFormationTile = spawnManager.spawnPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anchorPlayerAngle = (player.transform.position - assignedSpawnAnchor.transform.position).normalized;
        guardPos = (Vector2)assignedSpawnAnchor.transform.position + anchorPlayerAngle * 4f;
        directionToGuard = (guardPos - (Vector2)transform.position).normalized;
        switch (currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Death:
                Death();
                break;

            case EnemyState.Reposition:
                Reposition();
                break;

            case EnemyState.Retreat:
                Retreat();
                break;
        }
    }

    private void Attack()
    {
        if (Vector2.Distance((Vector2)transform.position, player.position) >= 18)
        {
            currentState = EnemyState.Chase;
        }

        switch (role)
        {
            case EnemyRoles.Warrior:
                if (Vector2.Distance((Vector2)transform.position, guardPos) >= 3f)
                {
                    currentState = EnemyState.Reposition;
                }
                break;
            case EnemyRoles.Archer:
                if (Vector2.Distance((Vector2)transform.position, player.position) <= 5f)
                {
                    currentState = EnemyState.Retreat;
                }
                break;
        }
    }
    private void Chase()
    {
        
        Vector2 chaseDirection = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
        rb.MovePosition((Vector2)transform.position + chaseDirection * speed * Time.fixedDeltaTime);
        if (Vector2.Distance((Vector2)transform.position,player.position) <= 12)
        {
            currentState = EnemyState.Attack;
        }
    }
    private void Death()
    {
        //play death anim
        Destroy(gameObject);
    }
    private void Reposition()
    {

        anchorPlayerAngle = (player.transform.position - assignedSpawnAnchor.transform.position).normalized;
        guardPos = (Vector2)assignedSpawnAnchor.transform.position + anchorPlayerAngle * 4f;
        directionToGuard = (guardPos - (Vector2)transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + directionToGuard * speed * Time.fixedDeltaTime);
        if (Vector2.Distance((Vector2)transform.position,guardPos) <= .5f)
        {
            currentState = EnemyState.Attack;
        }
    }
    private void Retreat()
    {
        if (hasStartedRetreating == false)
        {
            retreatStartPos = transform.position;
            hasStartedRetreating = true;
        }
        Vector2 anchorPlayerAngle = (assignedSpawnAnchor.transform.position - player.transform.position).normalized;
        retreatPos = (Vector2)assignedSpawnAnchor.transform.position + anchorPlayerAngle * 4f;
        directionToRetreat = ((Vector2)transform.position - retreatPos).normalized;
        rb.MovePosition((Vector2)transform.position + directionToRetreat * speed * Time.fixedDeltaTime);
        if (Vector2.Distance(retreatStartPos,transform.position) >= 3)
        {
            currentState = EnemyState.Attack;
            hasStartedRetreating = false;
        }
        
    }

    private void AnchorChase()
    {

    }
}
