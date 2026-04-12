using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
    public bool isEmberActive = true; // true = ember alive, false = ember depleted
    public int waveNumber = 0;
    private SpawnManager spawnManager;
    public int aliveEnemies;
    private bool swarmSpawning = false;
    private float swarmSpawnFreq = .75f;
    public bool isFightNodeActive = false;

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        emberComp = playerMovement.GetComponent<Light2D>();
        StartCoroutine("DepleteEmber");
        NewWave();
    }

    void Update()
    {
        if (emberAmount > baseEmber)
        {
            emberAmount = baseEmber;
        }
        if (emberAmount == 0)
        {
            
            if (!swarmSpawning)
            {
                swarmSpawning = true;
                StartCoroutine("SpawnSwarm");
            }
        }
        emberComp.pointLightOuterRadius = ((float)emberAmount / baseEmber) * lightRadius;
        emberUI.transform.localScale = new Vector3((float)emberAmount / baseEmber, (float)emberAmount / baseEmber, 0);
    }

    IEnumerator DepleteEmber()
    {
        while (isEmberActive)
        {
            if (emberAmount > 0)
            {
                
                yield return new WaitForSeconds(.1f);
                emberAmount -= .1f;
                if (emberAmount < 0) emberAmount = 0;
            }
            else
            {
                isEmberActive = false;
                yield return null;
            }
        }
        emberComp.pointLightOuterRadius = 0;
    }

    public void AddEmber(int emberToAdd)
    {
        isEmberActive = true;
        if (emberAmount == 0)
        {
            StartCoroutine("DepleteEmber");
        }

        emberAmount += emberToAdd;
        if (emberAmount > baseEmber) emberAmount = baseEmber;
    }

    public void NewWave()
    {
        waveNumber++;
        if (waveNumber > 1)
        {
            foreach (GameObject barrier in GameObject.FindGameObjectsWithTag("Barrier"))
            {
                barrier.GetComponent<BarrierBehaviour>().RetractSpikesBarrierAnim();
            }
            foreach (GameObject anchor in GameObject.FindGameObjectsWithTag("Anchor"))
            {
                anchor.GetComponent<FormationAnchorBehaviour>().DestroyAnchor();
            }
            ClearREEs();
        }
        spawnManager.GetRandomViableSpawn();
        if (waveNumber>1 && waveNumber < 10)
        {
            REEManager reeManager = FindFirstObjectByType<REEManager>();
            reeManager.StartCoroutine("InitializeSpawnPoints");
        }

        isFightNodeActive = false;
    }

    IEnumerator SpawnSwarm()
    {
        if (!isEmberActive)
        {
            spawnManager.SpawnSwarm();
            yield return new WaitForSeconds(swarmSpawnFreq);
            StartCoroutine("SpawnSwarm");
        }
        else
        {
            swarmSpawning = false;
            yield break;
        }
    }

    public void ClearREEs()
    {
        GameObject[] reeList = GameObject.FindGameObjectsWithTag("REE");
        foreach (GameObject reePoint in reeList)
        {
            Destroy(reePoint);
        }
    }
}