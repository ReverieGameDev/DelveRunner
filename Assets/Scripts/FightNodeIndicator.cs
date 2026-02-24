using UnityEngine;

public class FightNodeIndicator : MonoBehaviour
{
    
    private PlayerMovement playerMovement;
    private SpawnManager spawnManager;
    public Vector2 currentActiveFightNodeCoords;
    private Vector2 playerPosition;
    private Vector3 indicatorPosition;
    private Vector3 activeFightNodeCoords;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isFightNodeActive)
        {
            playerPosition = playerMovement.transform.position;
            activeFightNodeCoords = new Vector3(currentActiveFightNodeCoords.x, currentActiveFightNodeCoords.y);
            indicatorPosition = ((activeFightNodeCoords - playerMovement.transform.position).normalized * 2) + playerMovement.transform.position;
            Vector3 indicatorDirection = indicatorPosition - playerMovement.transform.position;
            float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
