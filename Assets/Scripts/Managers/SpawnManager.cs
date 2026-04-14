using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class SpawnManager : MonoBehaviour
{
    // ===== REFERENCES =====
    public GameObject[] EnemyArray;  // 0 = archer, 1 = warrior, etc.
    public GameObject[] BossArray;
    private MapRenderer mapRenderer;
    private PlayerCombat playerCombat;
    private EmberSystem emberSystem;
    public GameObject swarm;
    private MapGenerator mapGenerator;
    public Vector2 fightNodeCenter;
    private Vector2 fightNodeCenterOffsetted;
    public GameObject spawnEnemiesDetector;
    private FightNodeIndicator fightNodeIndicator;
    public bool isFightNodeActive = false;
    public GameObject spawnAnchor;
    private List<Room> listOfFightNodes = new List<Room>();
    private Room lastFNPicked;
    private int oneOf3Waves;
    private int[] formationsPerWave = { 2, 2, 2, 2, 3, 3, 3, 3, 4 };
    private List<Vector2> formationDirectionalOffset = new List<Vector2>();
    private Vector2 spawnPosOffset;

    // ===== SPAWN LOCATIONS =====
    public int[,] viableSpawnCenters;  // Filled by MapRenderer, marks valid spawn points

    // ===== FORMATION DATA =====
    // Dictionary holds all formations by name
    // Values: 0 = empty, 1 = archer, 2 = warrior (match EnemyArray indices)
    private Dictionary<string, int[,]> formations = new Dictionary<string, int[,]>()
{
    // === FIGHT NODE 1 (waves 1-3) ===
    { "1", new int[,] {
        { 0, 2, 0 },
        { 0, 1, 0 },
        { 0, 0, 0 } } },
    { "2", new int[,] {
        { 0, 1, 0 },
        { 0, 2, 0 },
        { 0, 0, 0 } } },
    { "3", new int[,] {
        { 0, 0, 0 },
        { 1, 0, 2 },
        { 0, 0, 0 } } },

    // === FIGHT NODE 2 (waves 4-6) ===
    { "4", new int[,] {
        { 0, 2, 0 },
        { 1, 0, 1 },
        { 0, 0, 0 } } },
    { "5", new int[,] {
        { 1, 0, 1 },
        { 0, 2, 0 },
        { 0, 0, 0 } } },
    { "6", new int[,] {
        { 0, 2, 0 },
        { 0, 0, 0 },
        { 1, 0, 1 } } },

    // === FIGHT NODE 3 (waves 7-9) ===
    { "7", new int[,] {
        { 2, 0, 2 },
        { 0, 0, 0 },
        { 1, 0, 1 } } },
    { "8", new int[,] {
        { 1, 0, 1 },
        { 0, 0, 0 },
        { 2, 0, 2 } } },
    { "9", new int[,] {
        { 2, 0, 1 },
        { 0, 0, 0 },
        { 1, 0, 2 } } },

    // === FIGHT NODE 4 (waves 10-12) ===
    { "10", new int[,] {
        { 0, 2, 0 },
        { 0, 4, 0 },
        { 0, 0, 0 } } },
    { "11", new int[,] {
        { 0, 4, 0 },
        { 0, 2, 0 },
        { 0, 0, 0 } } },
    { "12", new int[,] {
        { 0, 0, 0 },
        { 2, 0, 4 },
        { 0, 0, 0 } } },

    // === FIGHT NODE 5 (waves 13-15) ===
    { "13", new int[,] {
        { 0, 2, 0 },
        { 0, 1, 0 },
        { 0, 4, 0 } } },
    { "14", new int[,] {
        { 0, 4, 0 },
        { 0, 1, 0 },
        { 0, 2, 0 } } },
    { "15", new int[,] {
        { 0, 2, 0 },
        { 4, 0, 1 },
        { 0, 0, 0 } } },

    // === FIGHT NODE 6 (waves 16-18) ===
    { "16", new int[,] {
        { 2, 2, 2 },
        { 0, 1, 0 },
        { 0, 4, 0 } } },
    { "17", new int[,] {
        { 0, 4, 0 },
        { 0, 1, 0 },
        { 2, 2, 2 } } },
    { "18", new int[,] {
        { 2, 0, 4 },
        { 2, 1, 0 },
        { 2, 0, 0 } } },

    // === FIGHT NODE 7 (waves 19-21) ===
    { "19", new int[,] {
        { 2, 2, 2 },
        { 1, 1, 1 },
        { 0, 0, 0 } } },
    { "20", new int[,] {
        { 1, 1, 1 },
        { 0, 0, 0 },
        { 2, 2, 2 } } },
    { "21", new int[,] {
        { 2, 1, 0 },
        { 2, 1, 0 },
        { 2, 1, 0 } } },

    // === FIGHT NODE 8 (waves 22-24) ===
    { "22", new int[,] {
        { 2, 2, 2 },
        { 2, 4, 2 },
        { 1, 0, 1 } } },
    { "23", new int[,] {
        { 1, 0, 1 },
        { 2, 4, 2 },
        { 2, 2, 2 } } },
    { "24", new int[,] {
        { 2, 2, 1 },
        { 2, 4, 0 },
        { 2, 2, 1 } } },

    // === FIGHT NODE 9 (waves 25-27) ===
    { "25", new int[,] {
        { 2, 2, 2 },
        { 4, 1, 4 },
        { 0, 1, 0 } } },
    { "26", new int[,] {
        { 0, 1, 0 },
        { 4, 1, 4 },
        { 2, 2, 2 } } },
    { "27", new int[,] {
        { 2, 4, 0 },
        { 2, 1, 1 },
        { 2, 4, 0 } } },
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
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        fightNodeIndicator = FindFirstObjectByType<FightNodeIndicator>();
        emberSystem = FindFirstObjectByType<EmberSystem>();
        // Cache player position at start
        playerPos = playerCombat.transform.position;
        // Build viable spawn locations
        mapRenderer.ViableEnemySpawns();

        // Test spawn - remove this later
        
    }

    public void SpawnNextWave()
    {
        Debug.Log("SpawnNextWave called, waveNumber: " + emberSystem.waveNumber);
        formationDirectionalOffset.Clear();
        formationDirectionalOffset.Add(new Vector2(0, 24));             // north
        formationDirectionalOffset.Add(new Vector2(16.97f, 16.97f));    // northeast
        formationDirectionalOffset.Add(new Vector2(24, 0));             // east
        formationDirectionalOffset.Add(new Vector2(16.97f, -16.97f));   // southeast
        formationDirectionalOffset.Add(new Vector2(0, -24));            // south
        formationDirectionalOffset.Add(new Vector2(-16.97f, -16.97f));  // southwest
        formationDirectionalOffset.Add(new Vector2(-24, 0));            // west
        formationDirectionalOffset.Add(new Vector2(-16.97f, 16.97f));   // northwest
        if (emberSystem.waveNumber == 10)
        {
            SpawnBoss();
            return;
        }
        int amountOfFormations = formationsPerWave[emberSystem.waveNumber-1];
        for (int i = 0; i< amountOfFormations; i++)
        {
            PickfromPossibleWaveFormationPools();
            FormationDirectionOffset();
            SpawnWave(oneOf3Waves.ToString());
        }
    }

    private void PickfromPossibleWaveFormationPools()
    {
        oneOf3Waves = (emberSystem.waveNumber * 3) + Random.Range(-2, 1);
    }

    private void FormationDirectionOffset()
    {
        int randomDirection = Random.Range(0, formationDirectionalOffset.Count);
        fightNodeCenterOffsetted = new Vector2(fightNodeCenter.x + formationDirectionalOffset[randomDirection].x, fightNodeCenter.y + formationDirectionalOffset[randomDirection].y);
        formationDirectionalOffset.RemoveAt(randomDirection);
        
    }

    // ===== MAIN SPAWN METHOD =====
    public void SpawnWave(string waveFormation)
    {
        playerPos = playerCombat.transform.position;
        spawnPosArray = (int[,])formations[waveFormation].Clone();
        CalculateRotation(fightNodeCenterOffsetted);
        RotateSpawnPos();
        SpawnFormation(new Vector2(fightNodeCenterOffsetted.x, fightNodeCenterOffsetted.y));
    }

    // ===== SPAWN FORMATION =====
    private void SpawnFormation(Vector2 spawnPos)
    {
        GameObject currentSpawnAnchor = Instantiate(spawnAnchor, new Vector2(spawnPos.x, spawnPos.y), Quaternion.identity);
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
                Instantiate(EnemyArray[enemyType - 1], worldPos, Quaternion.identity).GetComponent<EnemyAI>().assignedSpawnAnchor = currentSpawnAnchor;
          
        }
        }
    }

    // ===== GET RANDOM SPAWN LOCATION =====
    public Vector2 GetRandomViableSpawn()
    {
        if (emberSystem.waveNumber == 10)
        {
            for (int i = 0; i < mapGenerator.rooms.Count; i++)
            {
                if (mapGenerator.rooms[i].roomType == "boss")
                {
                    Vector2 bossSpawnCoords = new Vector2(mapGenerator.rooms[i].centerX, mapGenerator.rooms[i].centerY);
                    fightNodeIndicator.currentActiveFightNodeCoords = bossSpawnCoords;
                    Instantiate(spawnEnemiesDetector, bossSpawnCoords, Quaternion.identity);
                    isFightNodeActive = true;
                    return bossSpawnCoords;
                }
            }
        }

        if (listOfFightNodes.Count == 0)
        {
            for (int i = 0; i < mapGenerator.rooms.Count; i++)
            {
                if (mapGenerator.rooms[i].roomType == "fightNode")
                {
                    listOfFightNodes.Add(mapGenerator.rooms[i]);
                }
            }
            if (lastFNPicked != null)
            {
                listOfFightNodes.Remove(lastFNPicked);
            }
        }

        int randomIndex = Random.Range(0, listOfFightNodes.Count);
        Vector2 enemySpawnCoords = new Vector2(listOfFightNodes[randomIndex].centerX, listOfFightNodes[randomIndex].centerY);
        fightNodeIndicator.currentActiveFightNodeCoords = enemySpawnCoords;
        Instantiate(spawnEnemiesDetector, enemySpawnCoords, Quaternion.identity);
        lastFNPicked = listOfFightNodes[randomIndex];
        listOfFightNodes.RemoveAt(randomIndex);
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
        Debug.Log(spawnPos + "<-spawn pos playerpos-> " + playerPos);
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
            newArray[0, 0] = spawnPosArray[0, 1];  // reverse direction
            newArray[0, 1] = spawnPosArray[0, 2];
            newArray[0, 2] = spawnPosArray[1, 2];
            newArray[1, 2] = spawnPosArray[2, 2];
            newArray[2, 2] = spawnPosArray[2, 1];
            newArray[2, 1] = spawnPosArray[2, 0];
            newArray[2, 0] = spawnPosArray[1, 0];
            newArray[1, 0] = spawnPosArray[0, 0];
            newArray[1, 1] = spawnPosArray[1, 1];

            spawnPosArray = newArray;
        }
    }


    public void SpawnSwarm()
    {
        Vector2 playerPos = FindFirstObjectByType<PlayerCombat>().transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * 60f;
        Instantiate(swarm, playerPos + randomOffset, Quaternion.identity);
    }
    public void SpawnBoss()
    {
        Room bossRoom = null;
        for (int i = 0; i < mapGenerator.rooms.Count; i++)
        {
            if (mapGenerator.rooms[i].roomType == "boss")
            {
                bossRoom = mapGenerator.rooms[i];
                break;
            }
        }
        Instantiate(BossArray[0], new Vector2(bossRoom.centerX, bossRoom.centerY), Quaternion.identity);
    }
}