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
    public Sprite multiShotIcon;
    public Sprite arrowVolleyIcon;
    public Sprite arrowRainIcon; 
    public float attackWindupTime;
    private float attackSpeed = 1f;//placeholder, will actually grab skeleton's attack speed

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
        //first we send the icon and windup time to the enemyattackindicator and wait for the indicator to fill up
        if (currentAttack == "arrowShot") { attackWindupTime = .75f; enemyAttackIndicator.SetIndicator(arrowShotIcon, attackWindupTime); }
        if (currentAttack == "arrowVolley") { attackWindupTime = 1.5f; enemyAttackIndicator.SetIndicator(arrowVolleyIcon, attackWindupTime); }
        if (currentAttack == "multiShot") { attackWindupTime = 1.25f; enemyAttackIndicator.SetIndicator(multiShotIcon, attackWindupTime); }
        if (currentAttack == "arrowRain") { attackWindupTime = .75f; enemyAttackIndicator.SetIndicator(arrowRainIcon, attackWindupTime); }
        yield return new WaitForSeconds(attackWindupTime); //waiting for indicator to fill
        //now we actually execute the attack
        if (currentAttack == "arrowShot") { ArrowShot(); }
        if (currentAttack == "arrowVolley") { ArrowVolley(); }
        if (currentAttack == "multiShot") { MultiShot(); }
        if (currentAttack == "arrowRain") { ArrowRain(); }
        yield return new WaitForSeconds(attackSpeed);//after exectuing the attack we put the skeleton's next attack on cooldown
        isReadyTofire = true;//then we fire again.
    }

    public string RandomSkeletonArcherAttack()
    {
        int randomAttack = Random.Range(0, attackList.Count);
        string currentAttack;
        return currentAttack = attackList[randomAttack];
    }



    private void ArrowShot() 
    {
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().Arrow("arrowShot");
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
