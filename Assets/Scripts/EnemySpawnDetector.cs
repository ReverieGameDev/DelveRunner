using UnityEngine;

public class EnemySpawnDetector : MonoBehaviour
{
    private SpawnManager spawnManager;
    private Timer timer;
    

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        timer = FindFirstObjectByType<Timer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spawnManager.spawnPos = transform.position;
            spawnManager.SpawnNextWave();
            timer.enemiesHaveSpawned = true;
            Destroy(gameObject);
        }
    }
}