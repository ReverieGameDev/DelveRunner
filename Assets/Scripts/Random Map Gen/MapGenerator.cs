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
    public int leftBound = 20;
    public int rightBound = 280;
    public int bottomBound = 20;
    public int topBound = 280;
    private Room lastViableSpawn;

    void Start()
    {
        mapRenderer = FindFirstObjectByType<MapRenderer>();
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
        mapRenderer.RenderMap();
    }

    public void PlaceRooms()
    {
        // Spawn room - center of map
        int spawnX = (leftBound + rightBound) / 2;
        int spawnY = (bottomBound + topBound) / 2;
        Room spawnRoom = new Room { centerX = spawnX, centerY = spawnY, radius = 15, roomType = "spawn" };
        rooms.Add(spawnRoom);
        CarveRoom(spawnX, spawnY, 15);

        // Fight Nodes
        for (int i = 0; i < Random.Range(2, 4); i++)
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
                    if (Vector2.Distance(newRoom, roomToCheckAgainst) > 40)
                    {
                        Room currentRoom = new Room { centerX = roomX, centerY = roomY, radius = 20, roomType = "fightNode" };
                        lastViableSpawn = currentRoom;
                        viableSpawn++;
                    }
                }

                if (viableSpawn == rooms.Count)
                {
                    rooms.Add(lastViableSpawn);
                    CarveRoom(roomX, roomY, 20);
                    roomPlaced = true;
                }
            }
            while (!roomPlaced);
        }

        // Caches
        for (int i = 0; i < Random.Range(1, 3); i++)
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
                    if (Vector2.Distance(newRoom, roomToCheckAgainst) > 10)
                    {
                        viableSpawn++;
                    }
                }

                if (viableSpawn == rooms.Count)
                {
                    Room currentRoom = new Room { centerX = roomX, centerY = roomY, radius = 5, roomType = "cache" };
                    rooms.Add(currentRoom);
                    CarveRoom(roomX, roomY, 5);
                    roomPlaced = true;
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
                if (Vector2.Distance(newRoom, roomToCheckAgainst) > 30)
                {
                    viableSpawn++;
                }
            }

            if (viableSpawn == rooms.Count)
            {
                Room bossRoom = new Room { centerX = roomX, centerY = roomY, radius = 25, roomType = "boss" };
                rooms.Add(bossRoom);
                CarveRoom(roomX, roomY, 25);
                bossPlaced = true;
            }
        }
        while (!bossPlaced);
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