using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // ===== REFERENCES =====
    private SpawnManager spawnManager;
    private WaveManager waveManager;
    private SendToAufburn sendToAufburn;
    private bool fightTimer = true;
    private bool overtime = false;
    private bool restTimer = false;
    private int fightTimeBase = 120;
    private int restTimeBase = 30;
    private int fightTime = 11;
    private int restTime = 30;
    public int waveNumber = 1;
    public TextMeshProUGUI timer;
    private EnemySpawnDetector enemySpawnDetector;
    public bool enemiesHaveSpawned = false;
    private bool spawnManagerHasSpawned = false;
    private float overtimeSpawnerBase = 5f;
    private float overtimeSpawner = 5f;

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        waveManager = FindFirstObjectByType<WaveManager>();
        sendToAufburn = FindFirstObjectByType<SendToAufburn>();
        enemySpawnDetector = FindAnyObjectByType<EnemySpawnDetector>();
        TimeManager();
    }

    private void TimeManager()
    {
        if (fightTimer == true)
        {
            if (!spawnManagerHasSpawned)
            {
                spawnManager.GetRandomViableSpawn();
                spawnManagerHasSpawned = true;
            }
            StartCoroutine("FightTimer");
        }
        else if (restTimer == true)
        {
            StartCoroutine("RestTimer");
        }
        else if (overtime == true)
        {
            StartCoroutine("Overtime");
        }
    }

    public IEnumerator FightTimer()
    {
        if (fightTime <= 0)
        {
            Debug.Log(enemiesHaveSpawned);
            if (enemiesHaveSpawned && waveManager.CountAliveEnemies() == 0)
            {
                fightTimer = false;
                restTimer = true;
                overtime = false;
                fightTime = fightTimeBase;
                enemiesHaveSpawned = false;
                TimeManager();
                yield break;
            }
            else if (!enemiesHaveSpawned || waveManager.CountAliveEnemies() != 0 && enemiesHaveSpawned)
            {
                fightTimer = false;
                restTimer = false;
                overtime = true;
                fightTime = fightTimeBase;
                TimeManager();
                yield break;
            }
        }
        if (waveManager.CountAliveEnemies() == 0 && enemiesHaveSpawned)
        {
            fightTimer = false;
            restTimer = true;
            overtime = false;
            fightTime = fightTimeBase;
            enemiesHaveSpawned = false;
            TimeManager();
            yield break;
        }
        int secs = fightTime % 60;
        timer.text = (fightTime / 60 + ":" + (secs < 10 ? "0" + secs : secs.ToString()));
        fightTime--;
        yield return new WaitForSeconds(1f);
        TimeManager();
    }

    public IEnumerator RestTimer()
    {
        if (restTime <= 0)
        {
            waveNumber++;
            if (waveNumber > 10)
            {
                sendToAufburn.StartCoroutine("SendBackToAufburn");
                yield break;
            }
            fightTimer = true;
            restTimer = false;
            restTime = restTimeBase;
            spawnManagerHasSpawned = false;
            TimeManager();
            yield break;
        }
        int secs = restTime % 60;
        timer.text = (restTime / 60 + ":" + (secs < 10 ? "0" + secs : secs.ToString()));
        restTime--;
        yield return new WaitForSeconds(1f);
        TimeManager();
    }

    public IEnumerator Overtime()
    {
        timer.text = "OVERTIME";
        overtimeSpawner *= .8f;
        spawnManager.SpawnSwarm();
        if (enemiesHaveSpawned && waveManager.CountAliveEnemies() == 0)
        {
            Debug.Log("ALL ENEMIES HAVE BEEN DEFEATED");
            overtime = false;
            restTimer = true;
            enemiesHaveSpawned = false;
            overtimeSpawner = overtimeSpawnerBase;
            TimeManager();
            yield break;
        }
        yield return new WaitForSeconds(overtimeSpawner);
        TimeManager();
    }

}