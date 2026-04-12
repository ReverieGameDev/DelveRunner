using System.Collections;
using UnityEngine;

public class EnemySpawnDetector : MonoBehaviour
{
    private SpawnManager spawnManager;
    private EmberSystem emberSystem;
    private bool playerInRange = false;
    public GameObject barrier;
    public GameObject fightNodeInterface;
    private AbilityManager abilityManager;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        abilityManager = FindFirstObjectByType<AbilityManager>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        emberSystem = FindFirstObjectByType<EmberSystem>();
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.playerFrozen = true;
            fightNodeInterface.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void AcceptFightNodeButton()
    {
        StartCoroutine("StartFightNodeSequence");
    }

    IEnumerator StartFightNodeSequence()
    {
        fightNodeInterface.SetActive(false);
        Color color = spriteRenderer.color;
        for (int i = 0; i < 100; i++)
        {
            color.a -= 0.01f;
            spriteRenderer.color = color;
            yield return new WaitForSecondsRealtime(.004f);
        }
        StartFightNode();
    }

    public void StartFightNode()
    {
        spawnManager.spawnPos = transform.position;
        Instantiate(barrier, new Vector2(transform.position.x + 4.5f, transform.position.y + 2.5f), Quaternion.identity);
        spawnManager.SpawnNextWave();
        emberSystem.isFightNodeActive = true;
        Time.timeScale = abilityManager.currentTimeScale;
        Destroy(gameObject);
        playerMovement.playerFrozen = false;
    }

    public void DeclineFightNode()
    {
        fightNodeInterface.SetActive(false);
        Time.timeScale = abilityManager.currentTimeScale;
        playerMovement.playerFrozen = false;
    }
}