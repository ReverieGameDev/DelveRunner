using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
    public EnvironmentThreat environmentThreat;
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
        StartFightNodeSequence();
    }

    private void StartFightNodeSequence()
    {
        environmentThreat.enabled = true;
        fightNodeInterface.SetActive(false);
        StartFightNode();
    }

    public void StartFightNode()
    {
        spawnManager.fightNodeCenter = transform.position;
        Instantiate(barrier, new Vector2(transform.position.x + 4.5f, transform.position.y + 2.5f), Quaternion.identity);
        GameObject squaresInstance = Instantiate(spawnManager.soloSquares, new Vector3(transform.position.x, transform.position.y + 5), Quaternion.identity);
        spawnManager.soloSquaresRefs = squaresInstance.GetComponentsInChildren<SoloSquares>();
        spawnManager.currentSoloSquares = squaresInstance;   // store for later destroy
        spawnManager.SpawnNextWave();
        emberSystem.isFightNodeActive = true;
        Time.timeScale = abilityManager.currentTimeScale;

        playerMovement.playerFrozen = false;
    }

    public void DeclineFightNode()
    {
        fightNodeInterface.SetActive(false);
        Time.timeScale = abilityManager.currentTimeScale;
        playerMovement.playerFrozen = false;
    }
}