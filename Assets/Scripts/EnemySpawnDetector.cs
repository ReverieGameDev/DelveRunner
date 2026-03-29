using UnityEngine;

public class EnemySpawnDetector : MonoBehaviour
{
    private SpawnManager spawnManager;
    private Timer timer;
    private bool hasTriggered = false;
    public GameObject barrier;
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
            Instantiate(barrier, new Vector2(transform.position.x+4.5f,transform.position.y+1), Quaternion.identity);
            spawnManager.SpawnNextWave();
            timer.enemiesHaveSpawned = true;
            Destroy(gameObject);
        }
    }
}