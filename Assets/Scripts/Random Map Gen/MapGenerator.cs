using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public GameObject nodeInstance;
    public int centerX;
    public int centerY;
    public int radius;
    public string roomType;
}

public class MapGenerator : MonoBehaviour
{
    public List<Vector2Int> corridorTiles = new List<Vector2Int>();
    public int mapHeight = 250;
    public int mapWidth = 350;
    public int[,] mapArray;
    private MapRenderer mapRenderer;
    public List<Room> rooms = new List<Room>();
    public List<Vector2> corridorMidpoints = new List<Vector2>();
    public List<Vector2> rerPositions = new List<Vector2>();
    public List<Vector2> rERList = new List<Vector2>();
    public int leftBound = 20;
    public int rightBound = 280;
    public int bottomBound = 20;
    public int topBound = 280;
    private Room lastViableSpawn;
    private RERManager rerManager;
    public List<Vector2Int> corridorCenterline = new List<Vector2Int>();

    // Room radii
    public int spawnRadius = 15;
    public int fightNodeRadius = 80;
    public int cacheRadius = 10;
    public int bossRadius = 45;

    // Minimum distance from other rooms
    public int fightNodeMinDistance = 140;
    public int cacheMinDistance = 10;
    public int bossMinDistance = 40;

    // Room counts
    public int fightNodeMin = 4;
    public int fightNodeMax = 5;
    public int cacheMin = 5;
    public int cacheMax = 8;
    public int rerFrequency = 6;
    private void Awake()
    {
        mapRenderer = FindFirstObjectByType<MapRenderer>();
        rerManager = FindFirstObjectByType<RERManager>();
    }
    void Start()
    {
        PopulateMap();
    }

    public void PopulateMap()
    {
        mapArray = new int[mapWidth, mapHeight];

        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                mapArray[t, i] = 0;
            }
        }

        PlaceRooms();
        PlaceCenterTilesCorridors();
        PlaceRER();
        SmoothEdges();
        RemoveIsolatedObstacles();
        RemoveUnwantedTiles();
        mapRenderer.RenderMap();
    }
    public void RemoveIsolatedObstacles()
    {
        int[,] tempMap = (int[,])mapArray.Clone();
        for (int i = 1; i < mapHeight - 1; i++)
        {
            for (int t = 1; t < mapWidth - 1; t++)
            {
                if (mapArray[t, i] == 0)
                {
                    int obstacleNeighbors = 0;
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            if (mapArray[t + x, i + y] == 0)
                                obstacleNeighbors++;
                        }
                    }
                    if (obstacleNeighbors < 3)
                        tempMap[t, i] = 1;
                }
            }
        }
        mapArray = tempMap;
    }
    public void PlaceCenterTilesCorridors()
    {
        int roomCount = rooms.Count;
        int[] connectionCount = new int[roomCount];
        int[] maxConnections = new int[roomCount];
        HashSet<string> existingCorridors = new HashSet<string>();

        for (int i = 0; i < roomCount; i++)
        {
            switch (rooms[i].roomType)
            {
                case "spawn": maxConnections[i] = 4; break;
                case "fightNode": maxConnections[i] = Random.Range(2, 4); break;
                case "cache": maxConnections[i] = 2; break;
                case "boss": maxConnections[i] = 2; break;
                default: maxConnections[i] = 2; break;
            }
        }

        bool[] inTree = new bool[roomCount];
        inTree[0] = true;

        for (int added = 1; added < roomCount; added++)
        {
            float bestDist = Mathf.Infinity;
            int bestFrom = -1;
            int bestTo = -1;

            for (int i = 0; i < roomCount; i++)
            {
                if (!inTree[i]) continue;
                for (int j = 0; j < roomCount; j++)
                {
                    if (inTree[j]) continue;
                    float dist = Vector2.Distance(
                        new Vector2(rooms[i].centerX, rooms[i].centerY),
                        new Vector2(rooms[j].centerX, rooms[j].centerY));
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestFrom = i;
                        bestTo = j;
                    }
                }
            }

            if (bestTo == -1) break;

            inTree[bestTo] = true;
            string key = Mathf.Min(bestFrom, bestTo) + "-" + Mathf.Max(bestFrom, bestTo);
            existingCorridors.Add(key);
            connectionCount[bestFrom]++;
            connectionCount[bestTo]++;
            CarveCorridor(rooms[bestFrom], rooms[bestTo]);
        }

        for (int i = 0; i < roomCount; i++)
        {
            if (connectionCount[i] >= maxConnections[i]) continue;

            List<int> nearest = new List<int>();
            for (int j = 0; j < roomCount; j++)
            {
                if (j != i) nearest.Add(j);
            }
            nearest.Sort((a, b) =>
            {
                float distA = Vector2.Distance(new Vector2(rooms[i].centerX, rooms[i].centerY), new Vector2(rooms[a].centerX, rooms[a].centerY));
                float distB = Vector2.Distance(new Vector2(rooms[i].centerX, rooms[i].centerY), new Vector2(rooms[b].centerX, rooms[b].centerY));
                return distA.CompareTo(distB);
            });

            foreach (int j in nearest)
            {
                if (connectionCount[i] >= maxConnections[i]) break;
                if (connectionCount[j] >= maxConnections[j]) continue;

                string key = Mathf.Min(i, j) + "-" + Mathf.Max(i, j);
                if (existingCorridors.Contains(key)) continue;

                existingCorridors.Add(key);
                connectionCount[i]++;
                connectionCount[j]++;
                CarveCorridor(rooms[i], rooms[j]);
            }
        }
    }

    public void CarveCorridor(Room roomA, Room roomB)
    {
        Vector2 centerA = new Vector2(roomA.centerX, roomA.centerY);
        Vector2 centerB = new Vector2(roomB.centerX, roomB.centerY);
        Vector2 direction = (centerB - centerA).normalized;

        Vector2 edgeA = centerA + direction * (roomA.radius - 3);
        Vector2 edgeB = centerB - direction * (roomB.radius - 3);

        float totalDistance = Vector2.Distance(edgeA, edgeB);

        if (totalDistance <= 0) return;

        float randomREEPlacement = Random.Range(0.3f, 0.7f);
        int reeX = (int)Mathf.Lerp(edgeA.x, edgeB.x, randomREEPlacement);
        int reeY = (int)Mathf.Lerp(edgeA.y, edgeB.y, randomREEPlacement);
        corridorMidpoints.Add(new Vector2(reeX, reeY));

        // Separate RER position from midpoint
        float randomRERPlacement = Random.Range(0.15f, 0.85f);
        int rerX = (int)Mathf.Lerp(edgeA.x, edgeB.x, randomRERPlacement);
        int rerY = (int)Mathf.Lerp(edgeA.y, edgeB.y, randomRERPlacement);

        for (int i = 0; i <= totalDistance; i++)
        {
            float t = i / totalDistance;

            int x = (int)Mathf.Lerp(edgeA.x, edgeB.x, t);
            int y = (int)Mathf.Lerp(edgeA.y, edgeB.y, t);
            corridorCenterline.Add(new Vector2Int(x, y));

            // Wide carve for walkable ground
            for (int w = -3; w <= 3; w++)
            {
                for (int h = -3; h <= 3; h++)
                {
                    if (x + w >= 0 && x + w < mapWidth && y + h >= 0 && y + h < mapHeight)
                    {
                        mapArray[x + w, y + h] = 1;
                    }
                }
            }

            // Narrow cobblestone path
            for (int w = -2; w <= 1; w++)
            {
                for (int h = -2; h <= 1; h++)
                {
                    int cx = x + w;
                    int cy = y + h;
                    if (cx >= 0 && cx < mapWidth && cy >= 0 && cy < mapHeight)
                    {
                        corridorTiles.Add(new Vector2Int(cx, cy));
                    }
                }
            }
        }
        if (totalDistance > 30)
        {
            bool insideRoom = false;
            for (int r = 0; r < rooms.Count; r++)
            {
                float dist = Vector2.Distance(new Vector2(rerX, rerY), new Vector2(rooms[r].centerX, rooms[r].centerY));
                if (dist < rooms[r].radius + 15)
                {
                    insideRoom = true;
                    break;
                }
            }
            if (!insideRoom)
            {
                rerPositions.Add(new Vector2(rerX, rerY));
            }
        }
    }

    public void PlaceRER()
    {
        for (int i = 0; i < rerPositions.Count; i++)
        {
            if (Random.Range(0, 10) < rerFrequency)
            {
                rERList.Add(rerPositions[i]);
                CarveRoom((int)rerPositions[i].x, (int)rerPositions[i].y, 10);
            }
        }
        rerManager.RERSelection();
    }
    public void PlaceRooms()
    {
        int spawnX = (leftBound + rightBound) / 2;
        int spawnY = (bottomBound + topBound) / 2;
        Room spawnRoom = new Room { centerX = spawnX, centerY = spawnY, radius = spawnRadius, roomType = "spawn" };
        rooms.Add(spawnRoom);
        CarveRoom(spawnX, spawnY, spawnRadius);
        Debug.Log("Spawn room placed at: " + spawnX + ", " + spawnY);

        for (int i = 0; i < Random.Range(fightNodeMin, fightNodeMax); i++)
        {
            bool roomPlaced = false;
            do
            {
                if (roomPlaced) { break; }
                int viableSpawn = 0;
                int roomX = Random.Range(leftBound, rightBound);
                int roomY = Random.Range(bottomBound, topBound);
                Vector2 newRoom = new Vector2(roomX, roomY);
                for (int x = 0; x < rooms.Count; x++)
                {
                    Vector2 roomToCheckAgainst = new Vector2(rooms[x].centerX, rooms[x].centerY);
                    if (Vector2.Distance(newRoom, roomToCheckAgainst) > fightNodeMinDistance)
                    {
                        Room currentRoom = new Room { centerX = roomX, centerY = roomY, radius = fightNodeRadius, roomType = "fightNode" };
                        lastViableSpawn = currentRoom;
                        viableSpawn++;
                    }
                }
                if (viableSpawn == rooms.Count)
                {
                    rooms.Add(lastViableSpawn);
                    CarveRoom(roomX, roomY, fightNodeRadius);
                    lastViableSpawn.nodeInstance = mapRenderer.RenderFightNodePrefab(new Vector2(roomX, roomY));
                    roomPlaced = true;
                    mapRenderer.RenderFightNodePrefab(new Vector2(roomX, roomY));
                    Debug.Log("Fight node placed at: " + roomX + ", " + roomY);
                }
            }
            while (!roomPlaced);
        }

        for (int i = 0; i < Random.Range(cacheMin, cacheMax); i++)
        {
            bool roomPlaced = false;
            do
            {
                int viableSpawn = 0;
                int roomX = Random.Range(leftBound, rightBound);
                int roomY = Random.Range(bottomBound, topBound);
                Vector2 newRoom = new Vector2(roomX, roomY);
                for (int x = 0; x < rooms.Count; x++)
                {
                    Vector2 roomToCheckAgainst = new Vector2(rooms[x].centerX, rooms[x].centerY);
                    if (Vector2.Distance(newRoom, roomToCheckAgainst) > cacheMinDistance)
                    {
                        viableSpawn++;
                    }
                }
                if (viableSpawn == rooms.Count)
                {
                    Room currentRoom = new Room { centerX = roomX, centerY = roomY, radius = cacheRadius, roomType = "cache" };
                    rooms.Add(currentRoom);
                    CarveRoom(roomX, roomY, cacheRadius);
                    roomPlaced = true;
                    mapRenderer.RenderChests(new Vector2(roomX, roomY));
                    mapRenderer.RenderCachePrefab(new Vector2(roomX, roomY));
                    Debug.Log("Cache placed at: " + roomX + ", " + roomY);
                }
            }
            while (!roomPlaced);
        }

        bool bossPlaced = false;
        do
        {
            int viableSpawn = 0;
            int roomX = Random.Range(leftBound, rightBound);
            int roomY = Random.Range(bottomBound, topBound);
            Vector2 newRoom = new Vector2(roomX, roomY);
            for (int x = 0; x < rooms.Count; x++)
            {
                Vector2 roomToCheckAgainst = new Vector2(rooms[x].centerX, rooms[x].centerY);
                if (Vector2.Distance(newRoom, roomToCheckAgainst) > bossMinDistance)
                {
                    viableSpawn++;
                }
            }
            if (viableSpawn == rooms.Count)
            {
                Room bossRoom = new Room { centerX = roomX, centerY = roomY, radius = bossRadius, roomType = "boss" };
                rooms.Add(bossRoom);
                CarveRoom(roomX, roomY, bossRadius);
                mapRenderer.RenderBossNodePrefab(new Vector2(roomX - 5, roomY - 6));
                bossPlaced = true;
                Debug.Log("Boss room placed at: " + roomX + ", " + roomY);
            }
        }
        while (!bossPlaced);

        Debug.Log("Total rooms placed: " + rooms.Count);
    }

    public void CarveRoom(int centerX, int centerY, int radius)
    {
        float noiseOffset = Random.Range(0f, 10000f);

        for (int y = centerY - radius - 3; y <= centerY + radius + 3; y++)
        {
            for (int x = centerX - radius - 3; x <= centerX + radius + 3; x++)
            {
                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) continue;

                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                float noise = Mathf.PerlinNoise((x + noiseOffset) * 0.3f, (y + noiseOffset) * 0.3f);
                float adjustedRadius = radius + (noise * 4f) - 2f;

                if (distance < adjustedRadius)
                {
                    mapArray[x, y] = 1;
                }
            }
        }
        SmoothEdges();
    }
    public void SmoothEdges()
    {
        for (int passes = 0; passes < 3; passes++)
        {
            int[,] tempMap = (int[,])mapArray.Clone();
            for (int i = 1; i < mapHeight - 1; i++)
            {
                for (int t = 1; t < mapWidth - 1; t++)
                {
                    int walkableNeighbors = 0;
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            if (mapArray[t + x, i + y] == 1)
                                walkableNeighbors++;
                        }
                    }
                    if (walkableNeighbors >= 5)
                        tempMap[t, i] = 1;
                }
            }
            mapArray = tempMap;
        }
    }
    public void RemoveUnwantedTiles()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                if (mapArray[t, i] == 0)
                {
                    bool hasLeft = (t - 1 >= 0 && mapArray[t - 1, i] == 0);
                    bool hasRight = (t + 1 < mapWidth && mapArray[t + 1, i] == 0);
                    bool hasTop = (i + 1 < mapHeight && mapArray[t, i + 1] == 0);
                    bool hasBottom = (i - 1 >= 0 && mapArray[t, i - 1] == 0);
                    int sideCount = (hasLeft ? 1 : 0) + (hasRight ? 1 : 0) + (hasTop ? 1 : 0) + (hasBottom ? 1 : 0);

                    if (sideCount <= 1)
                    {
                        mapArray[t, i] = 1;
                    }
                }
            }
        }
    }
}