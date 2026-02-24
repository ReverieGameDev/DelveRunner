using UnityEngine;

public class EnemySpawnDetector : MonoBehaviour
{
    private SpawnManager spawnManager;
    private Timer timer;
    private bool hasTriggered = false;

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        timer = FindFirstObjectByType<Timer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            spawnManager.spawnPos = transform.position;
            spawnManager.SpawnNextWave();
            timer.enemiesHaveSpawned = true;
            Destroy(gameObject);
        }
    }
}