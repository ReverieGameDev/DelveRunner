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
    private Timer timer;
    private Image torchImage;
    public float frameRate = 0.1f;
    private int currentFrame = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isFightNodeActive && timer.waveNumber != 10)
        {
            playerPosition = playerMovement.transform.position;
            activeFightNodeCoords = new Vector3(currentActiveFightNodeCoords.x, currentActiveFightNodeCoords.y);
            indicatorPosition = ((activeFightNodeCoords - playerMovement.transform.position).normalized * 2) + playerMovement.transform.position;
            Vector3 indicatorDirection = indicatorPosition - playerMovement.transform.position;
            float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else if (timer.waveNumber == 10)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }


}
