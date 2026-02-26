using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // ===== REFERENCES =====
    public GameObject[] EnemyArray;  // 0 = archer, 1 = warrior, etc.
    public GameObject[] BossArray;
    private WaveManager waveManager;
    private MapRenderer mapRenderer;
    private PlayerCombat playerCombat;
    private Timer timer;
    public GameObject swarm;
    private MapGenerator mapGenerator;
    public Vector2 spawnPos;
    public GameObject spawnEnemiesDetector;
    private FightNodeIndicator fightNodeIndicator;
    public bool isFightNodeActive = false;
    
    // ===== SPAWN LOCATIONS =====
    public int[,] viableSpawnCenters;  // Filled by MapRenderer, marks valid spawn points

    // ===== FORMATION DATA =====
    // Dictionary holds all formations by name
    // Values: 0 = empty, 1 = archer, 2 = warrior (match EnemyArray indices)
    private Dictionary<string, int[,]> formations = new Dictionary<string, int[,]>()
    {
        { "3archers", new int[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 } } },
        { "3warriors", new int[,] {
            { 0, 0, 0 },
            { 2, 2, 2 },
            { 0, 0, 0 } } },
        { "frontline", new int[,] {
            { 2, 2, 2 },   // warriors in front (top row faces player)
            { 1, 1, 1 },   // archers behind
            { 0, 0, 0 } } },
        { "1archer", new int[,] {
            { 0, 0, 0 },   
            { 0, 1, 0 },   
            { 0, 0, 0 } } },
        { "1tank", new int[,] {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 2 } } },
        { "vformation", new int[,] {
            { 2, 0, 2 },
            { 0, 1, 0 },
            { 2, 0, 2 } } 
        
        
        }
    };

    // ===== ROTATION =====
    private int[,] spawnPosArray = new int[3, 3];  // Working array for current spawn
    private int rotations;  // How many 45-degree rotations to apply

    // ===== PLAYER TRACKING =====
    public Vector3 playerPos;

    void Start()
    {
        //references
        mapRenderer = FindFirstObjectByType<MapRenderer>();
        waveManager = FindFirstObjectByType<WaveManager>();
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        fightNodeIndicator = FindFirstObjectByType<FightNodeIndicator>();
        timer = FindFirstObjectByType<Timer>();
        // Cache player position at start
        playerPos = playerCombat.transform.position;

        // Build viable spawn locations
        mapRenderer.ViableEnemySpawns();

        // Test spawn - remove this later
        
    }

    public void SpawnNextWave()
    {
        Debug.Log("SpawnNextWave called at time: " + Time.time);
        int rngWave;
        Debug.Log("SpawnNextWave called, waveNumber: " + timer.waveNumber);
        if (timer.waveNumber == 1)//lol
        {
            rngWave = Random.Range(0, 2);
            if (rngWave == 0) { SpawnWave("1archer"); }
            if (rngWave == 1) { SpawnWave("1tank"); }
            Debug.Log("we should be spawning rn");
        }
        if (timer.waveNumber == 2)
        {
            rngWave = Random.Range(0, 2);
            if (rngWave == 0) { SpawnWave("1archer"); }
            if (rngWave == 1) { SpawnWave("1tank"); }
            Debug.Log("we should be spawning rn");
        }

    }

    // ===== MAIN SPAWN METHOD =====
    public void SpawnWave(string waveFormation)
    {
        // Update player position for rotation calculation
        playerPos = playerCombat.transform.position;

        // Clone formation from dictionary (so we don't modify original)
        spawnPosArray = (int[,])formations[waveFormation].Clone();



        // Calculate rotation based on player position
        CalculateRotation(spawnPos);

        // Apply rotation to formation
        RotateSpawnPos();

        // Spawn all enemies in formation
        SpawnFormation(spawnPos);
    }

    // ===== SPAWN FORMATION =====
    private void SpawnFormation(Vector2 spawnPos)
    {
        // Loop through 3x3 grid
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int enemyType = spawnPosArray[row, col];

                // Skip empty slots
                if (enemyType == 0) continue;

                // Calculate world position offset
                // col: 0,1,2 becomes -3,0,3
                // row: 0,1,2 becomes 3,0,-3
                int offsetX = (col - 1) * 3;
                int offsetY = (1 - row) * 3;

                // Spawn enemy (enemyType - 1 because array is 0-indexed)
                Vector3 worldPos = new Vector3(spawnPos.x + offsetX, spawnPos.y + offsetY);
                Instantiate(EnemyArray[enemyType - 1], worldPos, Quaternion.identity);

                Debug.Log("Spawning enemy type " + enemyType + " at: " + worldPos);
            }
        }
        Invoke("NotifyWaveManager", 0.1f);
       
    }

    // ===== GET RANDOM SPAWN LOCATION =====
    public Vector2 GetRandomViableSpawn()
    {
        List<Room> listOfFightNodes = new List<Room>();
        for (int i = 0; i < mapGenerator.rooms.Count; i++)
        {
            if (mapGenerator.rooms[i].roomType == "fightNode")
            {
                listOfFightNodes.Add(mapGenerator.rooms[i]);
            }
        }
        int randomIndex = Random.Range(0, listOfFightNodes.Count);
        Vector2 enemySpawnCoords = new Vector2(listOfFightNodes[randomIndex].centerX, listOfFightNodes[randomIndex].centerY);
        fightNodeIndicator.currentActiveFightNodeCoords = enemySpawnCoords;
        Instantiate(spawnEnemiesDetector, enemySpawnCoords, Quaternion.identity);
        isFightNodeActive = true;
        return enemySpawnCoords;
        
    }

    // ===== CALCULATE ROTATION =====
    // Determines how many 45-degree rotations needed to face player
    private void CalculateRotation(Vector2 spawnPos)
    {
        // Get angle from spawn point looking at player
        float angle = Mathf.Atan2(playerPos.y - spawnPos.y, playerPos.x - spawnPos.x) * Mathf.Rad2Deg;

        Debug.Log("Angle to player: " + angle);

        // 8 directions, 45 degrees each
        // Formation default faces UP (toward player when angle is 67.5 to 112.5)
        if (angle >= 67.5f && angle < 112.5f) rotations = 0;  // UP
        else if (angle >= 22.5f && angle < 67.5f) rotations = 1;  // UP-RIGHT
        else if (angle >= -22.5f && angle < 22.5f) rotations = 2;  // RIGHT
        else if (angle >= -67.5f && angle < -22.5f) rotations = 3;  // DOWN-RIGHT
        else if (angle >= -112.5f && angle < -67.5f) rotations = 4; // DOWN
        else if (angle >= -157.5f && angle < -112.5f) rotations = 5; // DOWN-LEFT
        else if (angle >= 157.5f || angle < -157.5f) rotations = 6; // LEFT
        else if (angle >= 112.5f && angle < 157.5f) rotations = 7;  // UP-LEFT
    }

    // ===== ROTATE FORMATION =====
    // Rotates spawnPosArray 45 degrees clockwise, repeated 'rotations' times
    private void RotateSpawnPos()
    {
        for (int r = 0; r < rotations; r++)
        {
            int[,] newArray = new int[3, 3];

            // 45-degree clockwise rotation pattern:
            // Each position shifts one step around the edge
            newArray[0, 1] = spawnPosArray[0, 0];  // top-left -> top-center
            newArray[0, 2] = spawnPosArray[0, 1];  // top-center -> top-right
            newArray[1, 2] = spawnPosArray[0, 2];  // top-right -> middle-right
            newArray[2, 2] = spawnPosArray[1, 2];  // middle-right -> bottom-right
            newArray[2, 1] = spawnPosArray[2, 2];  // bottom-right -> bottom-center
            newArray[2, 0] = spawnPosArray[2, 1];  // bottom-center -> bottom-left
            newArray[1, 0] = spawnPosArray[2, 0];  // bottom-left -> middle-left
            newArray[0, 0] = spawnPosArray[1, 0];  // middle-left -> top-left
            newArray[1, 1] = spawnPosArray[1, 1];  // center stays

            spawnPosArray = newArray;
        }

        Debug.Log("Rotations applied: " + rotations);
        Debug.Log("Formation after rotation: " +
            spawnPosArray[0, 0] + spawnPosArray[0, 1] + spawnPosArray[0, 2] + " | " +
            spawnPosArray[1, 0] + spawnPosArray[1, 1] + spawnPosArray[1, 2] + " | " +
            spawnPosArray[2, 0] + spawnPosArray[2, 1] + spawnPosArray[2, 2]);
    }
    private void NotifyWaveManager()
    {
        waveManager.FindAllEnemiesToDefeat();
        //timer.isWaveActive = true;
    }

    public void SpawnSwarm()
    {
        Vector2 playerPos = FindFirstObjectByType<PlayerCombat>().transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * 60f;
        Instantiate(swarm, playerPos + randomOffset, Quaternion.identity);
    }
    // ===== BOSS SPAWN =====
    /*public void SpawnBoss()
    {
        Instantiate(BossArray[waveManager.currentLevel - 1],
            new Vector3(transform.position.x, transform.position.y + 10),
            Quaternion.identity);
    }*/
}