using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationAnchorBehaviour : MonoBehaviour
{
    // ===== FACING / ROTATION (the one brain for the whole formation) =====
    public float facingDeg;                 // direction the head faces right now, always a multiple of 45
    public int totalRotationSteps;          // net 45-degree steps since spawn. positive = counterclockwise. EnemyAI reads this.
    public float rotationThreshold = 33f;   // player must drift this far off facing before we rotate

    // ===== EXISTING STATE =====
    public bool chargeAttack;
    public int backlineEnemiesLeftAlive;
    public bool canWarriorLeap = false;
    public float frontlineTotalHP;
    public float frontlineCurrentHP = 0;
    public float backlineTotalHP;
    private Transform player;
    public bool formationBroken = false;
    public List<GameObject> enemiesInFormation = new List<GameObject>();
    public enum FormationCheck
    {
        LowFrontline,
        DeadBackline
    }

    void Start()
    {

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }
    public void EvaluateFormationState(FormationCheck formationCheck)
    {
        switch(formationCheck)
        {
            case FormationCheck.LowFrontline:
                if (frontlineCurrentHP / frontlineTotalHP <= .25f)
                {
                    formationBroken = true;
                }
                break;
        }

    }
    public void FormationAnchorEnemySetup()
    {
        foreach (GameObject enemy in enemiesInFormation)
        {
            if (enemy.GetComponent<EnemyAI>().isFrontline)
            {
                frontlineTotalHP += enemy.GetComponent<Enemy>().maxEnemyHealth;
            }
            else
            {
                backlineTotalHP += enemy.GetComponent<Enemy>().maxEnemyHealth;
            }
        }
        frontlineCurrentHP = frontlineTotalHP;
    }
    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        float playerAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        float diff = Mathf.DeltaAngle(facingDeg, playerAngle);
        if (Mathf.Abs(diff) >= rotationThreshold)
        {
            int steps = Mathf.RoundToInt(diff / 45f);
            facingDeg = Mathf.DeltaAngle(0f, facingDeg + steps * 45f);   // stay wrapped in -180..180
            totalRotationSteps += steps;
        }
    }

    // SpawnManager calls this right after instantiating the anchor
    public void SetFacing(float degrees)
    {
        facingDeg = Mathf.DeltaAngle(0f, degrees);
        totalRotationSteps = 0;
    }

    public void DestroyAnchor()
    {
        Destroy(gameObject);
    }
}