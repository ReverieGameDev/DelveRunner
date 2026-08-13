using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FightNodeIndicator : MonoBehaviour
{
    
    private PlayerMovement playerMovement;
    private SpawnManager spawnManager;
    public Vector2 currentActiveFightNodeCoords;
    private Vector2 playerPosition;
    private Vector3 indicatorPosition;
    private Vector3 activeFightNodeCoords;
    private EmberSystem emberSystem;
    private Image torchImage;
    public float frameRate = 0.1f;
    private int currentFrame = 0;
    private Vector2 pointToFNCoords;
    private Vector2 basePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emberSystem = FindFirstObjectByType<EmberSystem>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnManager.isFightNodeActive && emberSystem.waveNumber != 10)
        {
            playerPosition = playerMovement.transform.position;
            activeFightNodeCoords = new Vector3(currentActiveFightNodeCoords.x, currentActiveFightNodeCoords.y);
            indicatorPosition = ((activeFightNodeCoords - playerMovement.transform.position).normalized * 2) + playerMovement.transform.position;
            Vector3 indicatorDirection = indicatorPosition - playerMovement.transform.position;
            float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;

            pointToFNCoords = (activeFightNodeCoords - playerMovement.transform.position).normalized * 4f + playerMovement.transform.position;
            basePosition = Vector2.Lerp(basePosition, pointToFNCoords, Time.deltaTime * 5f);
            float xRandomCoord = Mathf.PerlinNoise(Time.time, 0) - 0.5f;
            float yRandomCoord = (Mathf.PerlinNoise(0, Time.time + 100) - 0.5f);
            float distanceWobble = (Mathf.PerlinNoise(Time.time + 150f, 0) - 0.5f) * 0.5f;
            transform.position = new Vector2(basePosition.x + xRandomCoord + distanceWobble, basePosition.y + yRandomCoord);
        }
        else if (emberSystem.waveNumber == 10)
        {
            activeFightNodeCoords = new Vector3(currentActiveFightNodeCoords.x, currentActiveFightNodeCoords.y);
            indicatorPosition = ((activeFightNodeCoords - playerMovement.transform.position).normalized * 2) + playerMovement.transform.position;
            Vector3 indicatorDirection = indicatorPosition - playerMovement.transform.position;
            float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
            pointToFNCoords = (activeFightNodeCoords - playerMovement.transform.position).normalized * 4f + playerMovement.transform.position;
            basePosition = Vector2.Lerp(basePosition, pointToFNCoords, Time.deltaTime * 5f);
            float xRandomCoord = Mathf.PerlinNoise(Time.time, 0) - 0.5f;
            float yRandomCoord = (Mathf.PerlinNoise(0, Time.time + 100) - 0.5f);
            float distanceWobble = (Mathf.PerlinNoise(Time.time + 150f, 0) - 0.5f) * 0.5f;
            transform.position = new Vector2(basePosition.x + xRandomCoord + distanceWobble, basePosition.y + yRandomCoord);
        }
    }


}
