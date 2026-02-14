using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonArcher : MonoBehaviour
{
    List<string> attackList = new List<string>();
    public bool isReadyTofire = true;
    public GameObject arrow;
    private PlayerMovement playerMovement;
    private EnemyAttackIndicator enemyAttackIndicator;
    public Sprite arrowShotIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackList.Add("arrowShot");//single highspeed arrow
        attackList.Add("arrowVolley");//literally ashe w from league
        attackList.Add("multiShot");//shoots 3 times with a break between each
        attackList.Add("arrowRain");//aoe circle around the player
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyTofire)
        {
            isReadyTofire = false;
            StartCoroutine("SkeletonArcherAttackCycle");
        }
    }

    public IEnumerator SkeletonArcherAttackCycle()
    {
        string currentAttack = RandomSkeletonArcherAttack();
        if (currentAttack == "arrowShot") { ArrowShot(); }
        if (currentAttack == "arrowVolley") { ArrowVolley(); }
        if (currentAttack == "multiShot") { MultiShot(); }
        if (currentAttack == "arrowRain") { ArrowRain(); }
        yield return new WaitForSeconds(3.5f);
        isReadyTofire = true;
    }

    public string RandomSkeletonArcherAttack()
    {
        int randomAttack = Random.Range(0, attackList.Count);
        string currentAttack;
        return currentAttack = attackList[randomAttack];
    }

    private void ArrowShot() 
    {
        enemyAttackIndicator.SetIndicator(arrowShotIcon);
    }
    private void ArrowVolley()
    {

    }
    private void MultiShot()
    {

    }
    private void ArrowRain()
    {

    }
}
