using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmberSystem : MonoBehaviour
{
    public static EmberSystem Instance;
    public float emberAmount = 100;
    public int baseEmber = 100;
    private PlayerMovement playerMovement;
    private int lightRadius = 45;
    public Light2D emberComp;
    public bool isEmberActive = true; // true = ember alive, false = ember depleted
    public int waveNumber = 0;
    private SpawnManager spawnManager;
    public int aliveEnemies;
    private bool swarmSpawning = false;
    public float swarmSpawnFreq = 5f;
    public bool isFightNodeActive = false;
    public int swarmCount = 0;
    public int swarmCap = 10;
    public TextMeshProUGUI emberText;
    void Start()
    {
        Instance = this;
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        //emberComp = playerMovement.GetComponent<Light2D>();
        StartCoroutine("DepleteEmber");
        NewWave();
    }

    void Update()
    {
        if (emberAmount == 0)
        {
            if (!swarmSpawning)
            {
                swarmSpawning = true;
                StartCoroutine("SpawnSwarm");
            }
        }
        if (emberAmount > baseEmber)
        {
            emberAmount = baseEmber;
        }
        emberComp.pointLightOuterRadius = ((float)emberAmount / baseEmber) * lightRadius;
    }

    IEnumerator DepleteEmber()
    {
        while (isEmberActive)
        {
            if (emberAmount > 0)
            {
                
                yield return new WaitForSeconds(.1f);
                emberAmount -= .06f;
                if (emberAmount < 0) emberAmount = 0;
                UpdateEmberText();
            }
            else
            {
                isEmberActive = false;
                yield return null;
            }
        }
        emberComp.pointLightOuterRadius = 0;
        
    }
    public void UpdateEmberText()
    {
        emberText.text = ((int)emberAmount).ToString();
    }
    public void AddEmber(int emberToAdd)
    {
        emberAmount += emberToAdd;
        if (emberAmount > baseEmber) emberAmount = baseEmber;
        if (!isEmberActive)
        {
            isEmberActive = true;
            StartCoroutine("DepleteEmber");
        }
        UpdateEmberText();
    }

    public void NewWave()
    {
        waveNumber++;
        if (waveNumber > 1)
        {
            AddEmber(35);
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
        while (emberAmount <= 0)
        {
            if (swarmCount < swarmCap)
            {
                spawnManager.SpawnSwarm();
                swarmCount++;
            }
            yield return new WaitForSeconds(swarmSpawnFreq);
        }
        swarmSpawning = false;
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