using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private FightNodeIndicator fightNodeIndicator;
    public bool isFightNodeActive = false;
    public GameObject spawnAnchor;
    private List<Room> listOfFightNodes = new List<Room>();
    private Room lastFNPicked;
    private int oneOf3Waves;
    private int[] formationsPerWave = { 2, 2, 2, 2, 3, 3, 3, 3, 4 };
    private List<Vector2> formationDirectionalOffset = new List<Vector2>();
    private Vector2 spawnPosOffset;
    private int randomDirection;
    private List<int> availableDirections = new List<int>();
    public GameObject soloSquares;
    public SoloSquares[] soloSquaresRefs;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    private readonly Vector2[] directionOffsets = new Vector2[]
{
        new Vector2(0, 24),             // 0 north
        new Vector2(16.97f, 16.97f),    // 1 northeast
        new Vector2(24, 0),             // 2 east
        new Vector2(16.97f, -16.97f),   // 3 southeast
        new Vector2(0, -24),            // 4 south
        new Vector2(-16.97f, -16.97f),  // 5 southwest
        new Vector2(-24, 0),            // 6 west
        new Vector2(-16.97f, 16.97f),   // 7 northwest
};
    private int chosenDirection;                 // compass id 0-7, stable meaning
    private float currentFormationFacing;        // degrees the head should face at spawn
    // ===== SPAWN LOCATIONS =====
    public int[,] viableSpawnCenters;  // Filled by MapRenderer, marks valid spawn points

    // ===== FORMATION DATA =====
    // Dictionary holds all formations by name
    // Values: 0 = empty, 1 = archer, 2 = warrior (match EnemyArray indices)
    private Dictionary<string, int[,]> formations = new Dictionary<string, int[,]>()
{
    // === FIGHT NODE 1 (waves 1-3) ===
// === FIGHT NODE 1 (waves 1-3) — 1w, 1a ===
        { "1", new int[,] {
            { 0, 2, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 } } },
        { "2", new int[,] {
            { 0, 2, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 } } },
        { "3", new int[,] {
            { 0, 2, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 } } },

        // === FIGHT NODE 2 (waves 4-6) — 1w, 2a ===
        { "4", new int[,] {
            { 0, 2, 0 },
            { 1, 0, 1 },
            { 0, 0, 0 } } },
        { "5", new int[,] {        // FIXED
            { 0, 2, 0 },
            { 1, 0, 0 },
            { 0, 0, 1 } } },
        { "6", new int[,] {
            { 0, 2, 0 },
            { 0, 0, 0 },
            { 1, 0, 1 } } },

        // === FIGHT NODE 3 (waves 7-9) — 2w, 2a ===
        { "7", new int[,] {
            { 2, 0, 2 },
            { 0, 0, 0 },
            { 1, 0, 1 } } },
        { "8", new int[,] {        // FIXED
            { 2, 0, 2 },
            { 1, 0, 1 },
            { 0, 0, 0 } } },
        { "9", new int[,] {        // FIXED
            { 2, 0, 2 },
            { 0, 0, 1 },
            { 1, 0, 0 } } },

        // === FIGHT NODE 4 (waves 10-12) — 1w, 1s ===
        { "10", new int[,] {
            { 0, 2, 0 },
            { 0, 4, 0 },
            { 0, 0, 0 } } },
        { "11", new int[,] {       
            { 0, 2, 0 },
            { 0, 0, 0 },
            { 0, 4, 0 } } },
        { "12", new int[,] {       
            { 0, 2, 0 },
            { 4, 0, 0 },
            { 0, 0, 0 } } },

        // === FIGHT NODE 5 (waves 13-15) — 1w, 1a, 1s ===
        { "13", new int[,] {
            { 0, 2, 0 },
            { 0, 1, 0 },
            { 0, 4, 0 } } },
        { "14", new int[,] {       
            { 0, 2, 0 },
            { 0, 4, 0 },
            { 0, 1, 0 } } },
        { "15", new int[,] {
            { 0, 2, 0 },
            { 4, 0, 1 },
            { 0, 0, 0 } } },

        // === FIGHT NODE 6 (waves 16-18) — 3w, 1a, 1s ===
        { "16", new int[,] {
            { 2, 2, 2 },
            { 0, 1, 0 },
            { 0, 4, 0 } } },
        { "17", new int[,] {       
            { 2, 2, 2 },
            { 0, 4, 0 },
            { 0, 1, 0 } } },
        { "18", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 4, 0, 1 },
            { 0, 0, 0 } } },

        // === FIGHT NODE 7 (waves 19-21) — 3w, 3a ===
        { "19", new int[,] {
            { 2, 2, 2 },
            { 1, 1, 1 },
            { 0, 0, 0 } } },
        { "20", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 0, 0, 0 },
            { 1, 1, 1 } } },
        { "21", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 1, 0, 1 },
            { 0, 1, 0 } } },

        // === FIGHT NODE 8 (waves 22-24) — 5w, 2a, 1s ===
        { "22", new int[,] {
            { 2, 2, 2 },
            { 2, 4, 2 },
            { 1, 0, 1 } } },
        { "23", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 2, 0, 2 },
            { 1, 4, 1 } } },
        { "24", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 2, 4, 1 },
            { 2, 0, 1 } } },

        // === FIGHT NODE 9 (waves 25-27) — 3w, 2a, 2s ===
        { "25", new int[,] {
            { 2, 2, 2 },
            { 4, 1, 4 },
            { 0, 1, 0 } } },
        { "26", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 0, 1, 0 },
            { 4, 1, 4 } } },
        { "27", new int[,] {       // FIXED
            { 2, 2, 2 },
            { 1, 4, 1 },
            { 0, 4, 0 } } },
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
        availableDirections.Clear();
        for (int i = 0; i < 8; i++) availableDirections.Add(i);
        
        if (emberSystem.waveNumber == 10)
        {
            SpawnBoss();
            return;
        }
        int amountOfFormations = formationsPerWave[emberSystem.waveNumber - 1];
        for (int i = 0; i < amountOfFormations; i++)
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
        // pick a compass id and remove it - the ID keeps its meaning, unlike a shrinking-list index
        int pick = Random.Range(0, availableDirections.Count);
        chosenDirection = availableDirections[pick];
        availableDirections.RemoveAt(pick);

        Vector2 offset = directionOffsets[chosenDirection];
        fightNodeCenterOffsetted = new Vector2(fightNodeCenter.x + offset.x, fightNodeCenter.y + offset.y);

        // the head faces back toward the fight node center = opposite of the offset direction
        currentFormationFacing = Mathf.Atan2(-offset.y, -offset.x) * Mathf.Rad2Deg;
    }

    // ===== MAIN SPAWN METHOD =====
    public void SpawnWave(string waveFormation)
    {
        playerPos = playerCombat.transform.position;
        spawnPosArray = (int[,])formations[waveFormation].Clone();
        CalculateRotation();
        RotateSpawnPos();
        SpawnFormation(new Vector2(fightNodeCenterOffsetted.x, fightNodeCenterOffsetted.y));
    }
    private void CalculateRotation()
    {
        switch (chosenDirection)
        {
            case 0: rotations = 4; break; // north
            case 1: rotations = 3; break; // northeast
            case 2: rotations = 2; break; // east
            case 3: rotations = 1; break; // southeast
            case 4: rotations = 0; break; // south
            case 5: rotations = 7; break; // southwest
            case 6: rotations = 6; break; // west
            case 7: rotations = 5; break; // northwest
        }
    }
    // ===== SPAWN FORMATION =====
    private void SpawnFormation(Vector2 spawnPos)
    {
        GameObject currentSpawnAnchor = Instantiate(spawnAnchor, new Vector2(spawnPos.x, spawnPos.y), Quaternion.identity);
        FormationAnchorBehaviour currentAnchor = currentSpawnAnchor.GetComponent<FormationAnchorBehaviour>();
        currentAnchor.SetFacing(currentFormationFacing);

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int enemyType = spawnPosArray[row, col];
                if (enemyType == 0) continue;

                int offsetX = (col - 1) * 3;
                int offsetY = (1 - row) * 3;

                Vector3 worldPos = new Vector3(spawnPos.x + offsetX, spawnPos.y + offsetY);
                GameObject spawned = Instantiate(EnemyArray[enemyType - 1], worldPos, Quaternion.identity);
                spawned.GetComponent<EnemyAI>().assignedSpawnAnchor = currentSpawnAnchor;
                spawned.GetComponent<EnemyAI>().assignedSpawnAnchorScript = currentAnchor;
                
                currentAnchor.enemiesInFormation.Add(spawned);
                spawnedEnemies.Add(spawned);
            }
        }



        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy == null) continue;
            enemy.GetComponent<EnemyAI>().soloSquaresPrefab = soloSquaresRefs;
        }
        currentAnchor.FormationAnchorEnemySetup();
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
                    mapGenerator.rooms[i].nodeInstance.GetComponentInChildren<EnemySpawnDetector>().enabled = true;
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
        if (lastFNPicked != null)
        {
            lastFNPicked.nodeInstance.GetComponentInChildren<EnemySpawnDetector>().enabled = false;
        }
        listOfFightNodes[randomIndex].nodeInstance.GetComponentInChildren<EnemySpawnDetector>().enabled = true;
        lastFNPicked = listOfFightNodes[randomIndex];
        listOfFightNodes.RemoveAt(randomIndex);
        isFightNodeActive = true;
        return enemySpawnCoords;
    }

    // ===== CALCULATE ROTATION =====
    // Determines how many 45-degree rotations needed to face player
    private void CalculateRotation(Vector2 spawnPos)
    {
        switch (randomDirection)
        {
            case 0: rotations = 4; break; // north
            case 1: rotations = 3; break; // northeast
            case 2: rotations = 2; break; // east
            case 3: rotations = 1; break; // southeast
            case 4: rotations = 0; break; // south
            case 5: rotations = 7; break; // southwest
            case 6: rotations = 6; break; // west
            case 7: rotations = 5; break; // northwest
        }
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