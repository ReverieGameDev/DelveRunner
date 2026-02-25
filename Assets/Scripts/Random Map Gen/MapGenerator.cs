using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int centerX;
    public int centerY;
    public int radius;
    public string roomType;
}

public class MapGenerator : MonoBehaviour
{
    public int mapHeight = 500;
    public int mapWidth = 500;
    public int[,] mapArray;
    private MapRenderer mapRenderer;
    public List<Room> rooms = new List<Room>();
    public List<Vector2> corridorMidpoints = new List<Vector2>();
    public int leftBound = 20;
    public int rightBound = 280;
    public int bottomBound = 20;
    public int topBound = 280;
    private Room lastViableSpawn;

    // Room radii
    public int spawnRadius = 15;
    public int fightNodeRadius = 25;
    public int cacheRadius = 10;
    public int bossRadius = 30;

    // Minimum distance from other rooms
    public int fightNodeMinDistance = 40;
    public int cacheMinDistance = 10;
    public int bossMinDistance = 40;

    // Room counts
    public int fightNodeMin = 4;
    public int fightNodeMax = 5;
    public int cacheMin = 2;
    public int cacheMax = 5;
    private void Awake()
    {
        mapRenderer = FindFirstObjectByType<MapRenderer>();
        
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
        mapRenderer.RenderMap();
    }

    public void PlaceCenterTilesCorridors()
    {
        List<int> visited = new List<int>();
        List<int> orderedRooms = new List<int>();

        float closestDist = Mathf.Infinity;
        int closestIndex = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType == "cache" || rooms[i].roomType == "boss") continue;
            float dist = Vector2.Distance(Vector2.zero, new Vector2(rooms[i].centerX, rooms[i].centerY));
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }
        visited.Add(closestIndex);
        orderedRooms.Add(closestIndex);

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            float closestDistance = Mathf.Infinity;
            int nearestIndex = -1;
            for (int z = 0; z < rooms.Count; z++)
            {
                if (visited.Contains(z)) continue;
                if (rooms[z].roomType == "cache" || rooms[z].roomType == "boss") continue;
                float dist = Vector2.Distance(new Vector2(rooms[orderedRooms[i]].centerX, rooms[orderedRooms[i]].centerY), new Vector2(rooms[z].centerX, rooms[z].centerY));
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestIndex = z;
                }
            }

            if (nearestIndex == -1) break;

            visited.Add(nearestIndex);
            orderedRooms.Add(nearestIndex);
        }

        for (int i = 0; i < orderedRooms.Count - 1; i++)
        {
            CarveCorridor(rooms[orderedRooms[i]], rooms[orderedRooms[i + 1]]);
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomType != "cache" && rooms[i].roomType != "boss") continue;

            float closestDistance = Mathf.Infinity;
            int nearestIndex = 0;
            for (int z = 0; z < orderedRooms.Count; z++)
            {
                float dist = Vector2.Distance(new Vector2(rooms[i].centerX, rooms[i].centerY), new Vector2(rooms[orderedRooms[z]].centerX, rooms[orderedRooms[z]].centerY));
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestIndex = orderedRooms[z];
                }
            }
            CarveCorridor(rooms[i], rooms[nearestIndex]);
        }
    }

    public void CarveCorridor(Room roomA, Room roomB)
    {
        float totalDistance = Vector2.Distance(new Vector2(roomA.centerX, roomA.centerY), new Vector2(roomB.centerX, roomB.centerY));

        // Pick one REE point per corridor
        float randomREEPlacement = Random.Range(0.3f, 0.7f);
        int reeX = (int)Mathf.Lerp(roomA.centerX, roomB.centerX, randomREEPlacement);
        int reeY = (int)Mathf.Lerp(roomA.centerY, roomB.centerY, randomREEPlacement);
        corridorMidpoints.Add(new Vector2(reeX, reeY));

        for (int i = 0; i <= totalDistance; i++)
        {
            float t = i / totalDistance;
            int x = (int)Mathf.Lerp(roomA.centerX, roomB.centerX, t);
            int y = (int)Mathf.Lerp(roomA.centerY, roomB.centerY, t);
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
        }
    }

    public void PlaceRooms()
    {
        // Spawn room
        int spawnX = (leftBound + rightBound) / 2;
        int spawnY = (bottomBound + topBound) / 2;
        Room spawnRoom = new Room { centerX = spawnX, centerY = spawnY, radius = spawnRadius, roomType = "spawn" };
        rooms.Add(spawnRoom);
        CarveRoom(spawnX, spawnY, spawnRadius);
        Debug.Log("Spawn room placed at: " + spawnX + ", " + spawnY);

        // Fight Nodes
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
                    roomPlaced = true;
                    Debug.Log("Fight node placed at: " + roomX + ", " + roomY);
                }
            }
            while (!roomPlaced);
        }

        // Caches
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
                    Debug.Log("Cache placed at: " + roomX + ", " + roomY);
                }
            }
            while (!roomPlaced);
        }

        // Boss room
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