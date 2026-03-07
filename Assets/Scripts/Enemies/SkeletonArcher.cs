using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private LineRenderer lineRenderer;
    private bool isIndicatorActive = false;
    private string currentAttack;
    private Vector2 archerToPlayerAngle;
    public float indicatorLength = 20f;
    private int multiShotCount = 0;
    public List<GameObject> arrowVolleyList = new List<GameObject>();
    public int arrowVolleyAmount = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.numCornerVertices = 0;
        enemyAttackIndicator = GetComponentInChildren<EnemyAttackIndicator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        attackList.Add("arrowShot");//single highspeed arrow
        attackList.Add("arrowVolley");//literally ashe w from league
        attackList.Add("multiShot");//shoots 3 times with a break between each
        attackList.Add("arrowRain");//aoe circle around the player
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (isReadyTofire)
        {
            isReadyTofire = false;
            StartCoroutine("SkeletonArcherAttackCycle");
        }
        if (isIndicatorActive && currentAttack == "arrowShot")
        {
            archerToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (Vector3)archerToPlayerAngle * indicatorLength*3);
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.green;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
        }
        if (isIndicatorActive && currentAttack == "multiShot")
        {
            archerToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (Vector3)archerToPlayerAngle * indicatorLength*3);
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.green;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
        }
        if (isIndicatorActive && currentAttack == "arrowVolley")
        {
            lineRenderer.SetPosition(0, transform.position);
            for (int i = 0; i< arrowVolleyAmount; i++)
            {
                archerToPlayerAngle = new Vector2(playerMovement.transform.position.x - transform.position.x, playerMovement.transform.position.y - transform.position.y).normalized;
                float arrowAngle = Mathf.Rad2Deg * Mathf.Atan2(archerToPlayerAngle.y, archerToPlayerAngle.x) + 30 - (15 * i);
                arrowAngle = Mathf.Deg2Rad * arrowAngle;
                archerToPlayerAngle = new Vector2(Mathf.Cos(arrowAngle), Mathf.Sin(arrowAngle));
                lineRenderer.SetPosition(i * 2 + 1, transform.position + (Vector3)archerToPlayerAngle * indicatorLength*4);
                lineRenderer.SetPosition(i * 2 + 2, transform.position);
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                lineRenderer.startWidth = 1f;
                lineRenderer.endWidth = 1f;
            }
        }
        else if (!isIndicatorActive)
        {
            lineRenderer.enabled = false;
        }
    }

    public IEnumerator SkeletonArcherAttackCycle()
    {
        currentAttack = RandomSkeletonArcherAttack();
        //first we send the icon and windup time to the enemyattackindicator and wait for the indicator to fill up
        if (currentAttack == "arrowShot") 
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            isIndicatorActive = true;
            attackWindupTime = .75f; 
            enemyAttackIndicator.SetIndicator(arrowShotIcon, attackWindupTime); 
        
        }
        if (currentAttack == "arrowVolley") 
            {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = (arrowVolleyAmount*2)+1;
            isIndicatorActive = true;
            attackWindupTime = 2.5f; 
                enemyAttackIndicator.SetIndicator(arrowVolleyIcon, attackWindupTime); 
            }
        if (currentAttack == "multiShot") 
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            isIndicatorActive = true;
            attackWindupTime = 1.25f;
            enemyAttackIndicator.SetIndicator(multiShotIcon, attackWindupTime); 
        }
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

        int randomAttack = Random.Range(0, attackList.Count-1);
        string currentAttack;
        return currentAttack = attackList[randomAttack];
    }



    private void ArrowShot() 
    {
        isIndicatorActive = false;
        
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot",0);
    }
    private void ArrowVolley()
    {
        arrowVolleyList.Clear();
        isIndicatorActive = false;
        for (int i = 0; i < arrowVolleyAmount; i++)
        {
            int offset = 30 - (15 * i);
            GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
            spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowVolley", offset);
            arrowVolleyList.Add(spawnedArrow);
            
        }
    }
    private void MultiShot()
    {
        isIndicatorActive = false;
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot",0);
        multiShotCount++;
        StartCoroutine("MultiShotDelay");
    }
    private void ArrowRain()
    {

    }

    IEnumerator MultiShotDelay()
    {
        multiShotCount++;
        yield return new WaitForSeconds(.3f);
        GameObject spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<ArrowBehaviour>().AttackName("arrowShot",0);
        if (multiShotCount < 3)
        {
            
            StartCoroutine("MultiShotDelay");
        }
        else if(multiShotCount >= 3)
        {
            lineRenderer.enabled = false;
            isIndicatorActive = false;
            multiShotCount = 0;
            StopCoroutine("MultiShotDelay");
        }
    }
}
