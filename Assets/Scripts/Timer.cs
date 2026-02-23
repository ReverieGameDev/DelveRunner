using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerUI;
    public bool betweenWavesTimer = false;
    public bool duringWavesTimer = true;
    public int betweenWavesTimerCountdownBase = 15;
    public int betweenWavesTimerCountdown = 15;
    public int duringWavesTimerCountdown = 120;
    public int duringWavesTimerCountBase = 120;

    public int spawnCounter = 0;

    public int duringWavesTimerCountdownMinutes;
    public int betweenWavesTimerCountdownMinutes;

    public int waveNumber = 1;
    public bool isWaveActive = false;

    private bool hasSpawnedThisWave = false;
    private SpawnManager spawnManager;
    private WaveManager waveManager;
    private SendToAufburn sendToAufburn;

    void Start()
    {
        sendToAufburn = FindFirstObjectByType<SendToAufburn>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        waveManager = FindFirstObjectByType<WaveManager>();
        StartCoroutine("TimerTime");
    }

    IEnumerator TimerTime()
    {
        

        if (hasSpawnedThisWave == false)
        {
            spawnManager.SpawnNextWave();
            duringWavesTimerCountdown = duringWavesTimerCountBase;
            hasSpawnedThisWave = true;

            isWaveActive = true; // ✅ IMPORTANT
        }

        if (duringWavesTimer)
        {
            duringWavesTimerCountdown--;

            // ✅ FIX: minutes should come from countdown, not from itself
            duringWavesTimerCountdownMinutes = duringWavesTimerCountdown / 60;

            if (duringWavesTimerCountdown % 60 < 10)
            {
                timerUI.text = (duringWavesTimerCountdown / 60 + " : 0" + duringWavesTimerCountdown % 60);
            }
            else
            {
                timerUI.text = (duringWavesTimerCountdown / 60 + " : " + duringWavesTimerCountdown % 60);
            }

            if (duringWavesTimerCountdown <= 0 && waveManager.CountAliveEnemies() == 0)
            {
                // Wave time ended -> go to between-waves
                betweenWavesTimerCountdown = betweenWavesTimerCountdownBase;
                betweenWavesTimer = true;
                duringWavesTimer = false;

                isWaveActive = false; // ✅ wave timer ended

                StartCoroutine("BetweenWavesTimerTime");
                yield break; // ✅ stop this coroutine instance cleanly
            }
            else if (duringWavesTimerCountdown <= 0 && waveManager.CountAliveEnemies() != 0)
            {
                
                StartCoroutine("Overtime");
                yield break;
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine("TimerTime");
        }
    }
    public IEnumerator Overtime()
    {
        while (waveManager.CountAliveEnemies() != 0)
        {
            yield return new WaitForSeconds(1.5f);
            spawnCounter++;
            timerUI.color = Color.red;
            timerUI.text = ("OVERTIME");
            spawnManager.SpawnSwarm();
            // TODO: spawn swarm enemies here based on spawnCounter
        }

        // Enemies cleared during overtime
        betweenWavesTimerCountdown = betweenWavesTimerCountdownBase;
        betweenWavesTimer = true;
        duringWavesTimer = false;
        isWaveActive = false;
        timerUI.color = Color.white;
        StartCoroutine("BetweenWavesTimerTime");
    }
    public IEnumerator BetweenWavesTimerTime()
    {
        if (betweenWavesTimer)
        {
            betweenWavesTimerCountdown--;

            if (betweenWavesTimerCountdown % 60 < 10)
            {
                timerUI.text = (betweenWavesTimerCountdown / 60 + " : 0" + betweenWavesTimerCountdown % 60);
            }
            else
            {
                timerUI.text = (betweenWavesTimerCountdown / 60 + " : " + betweenWavesTimerCountdown % 60);
            }

            if (betweenWavesTimerCountdown <= 0)
            {
                // Wave starts
                waveNumber++;
                if (waveNumber == 11)
                {
                    sendToAufburn.StartCoroutine("SendBackToAufburn");
                }
                duringWavesTimerCountdown = duringWavesTimerCountBase;

                betweenWavesTimer = false;
                duringWavesTimer = true;

                hasSpawnedThisWave = false;

                
                StartCoroutine("TimerTime");
                yield break; // ✅ stop this coroutine instance cleanly
            }
            else
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine("BetweenWavesTimerTime");
            }
        }
    }
}
