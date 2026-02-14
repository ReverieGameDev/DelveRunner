using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 0;
    public int currentDelve = 1;
    public TextMeshProUGUI waveNumberGUI;
    private Timer timer;
    public GameObject[] enemiesToDefeat;

    void Start()
    {
        timer = FindAnyObjectByType<Timer>();
    }

    void Update()
    {
        FindAllEnemiesToDefeat(); // ✅ keep list updated (simple beginner approach)

        if (timer.isWaveActive && CountAliveEnemies() == 0)
        {
            // Wave cleared - tell timer to start break
            timer.isWaveActive = false;

            timer.duringWavesTimer = false; // ✅ switch mode
            timer.betweenWavesTimer = true; // ✅ switch mode

            timer.betweenWavesTimerCountdown = timer.betweenWavesTimerCountdownBase; // ✅ FIXED VARIABLE

            timer.StopCoroutine("TimerTime");
            timer.StartCoroutine("BetweenWavesTimerTime");
        }
    }

    public void FindAllEnemiesToDefeat()
    {
        for (int i = 0; i == timer.spawnCounter; i++)
        {
            enemiesToDefeat = GameObject.FindGameObjectsWithTag("Enemy");
        }
        
    }

    public int CountAliveEnemies()
    {
        int count = 0;
        for (int i = 0; i < enemiesToDefeat.Length; i++)
        {
            if (enemiesToDefeat[i] != null)
            {
                count++;
            }
        }
        return count;
    }
}
