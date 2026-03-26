using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmberSystem : MonoBehaviour
{
    public float emberAmount = 100;
    public int baseEmber = 100;
    public GameObject emberUI;
    private PlayerMovement playerMovement;
    private int lightRadius = 45;
    private Light2D emberComp;
    private bool emberDead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        emberComp = playerMovement.GetComponent<Light2D>();
        StartCoroutine("DepleteEmber");
    }

    // Update is called once per frame
    void Update()
    {
        if (emberAmount > baseEmber)
        {
            emberAmount = baseEmber;
        }
        if (emberAmount == 0)
        {
            emberDead = true;
        }
        emberComp.pointLightOuterRadius = ((float)emberAmount / baseEmber) * lightRadius;
        emberUI.transform.localScale = new Vector3((float)emberAmount / baseEmber, (float)emberAmount / baseEmber, 0);
    }

    IEnumerator DepleteEmber()
    {
        while (!emberDead)
        {
            if (emberAmount > 0)
            {
                yield return new WaitForSeconds(.025f);
                emberAmount -= .1f;
                if (emberAmount < 0) emberAmount = 0;
            }
            else
            {
                yield return null; // Wait a frame if ember is 0
            }
        }

        // Only runs once when emberDead becomes true
        emberComp.pointLightOuterRadius = 0;
    }

    public void AddEmber(int emberToAdd)
    {
        emberDead = false;

        emberAmount+=emberToAdd;
        Debug.Log(emberAmount);
        if (emberAmount > baseEmber) emberAmount = baseEmber;
        
    }
}
