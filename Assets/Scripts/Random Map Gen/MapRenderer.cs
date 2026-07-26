using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private SpawnManager spawnManager;
    public RuleTile cobbleRuleTile;
    public GameObject[] obeliskPrefabs;
    // Tilemaps
    public Tilemap walkableTilemap;
    public Tilemap obstacleTilemap;
    public Tilemap walkableDecorTilemap;
    public Tilemap oobTileMap;
    public Tilemap cobbleTilemap;

    // Floor tiles
    public TileBase[] floorTile;
    public TileBase[] darkGrassTile;
    public TileBase[] walkableDecorTile;
    public TileBase oobTile;
    public TileBase obstacleTile;

    // Obstacle decor
    public GameObject[] obstacleDecorTile;
    public TileBase[] obstacleDecorTile2;
    public TileBase[] obstacleDecorTile3;
    public GameObject[] edgeDecorObjects;
    // Edge tiles
    public TileBase leftEdge;
    public TileBase downEdge;
    public TileBase rightEdge;
    public TileBase upEdge;
    public TileBase bottomLeftEdge;
    public TileBase topLeftEdge;
    public TileBase bottomRightEdge;
    public TileBase topRightEdge;

    public List<GameObject> chests = new List<GameObject>();

    public GameObject cachePrefab; //make this into a list later, you need 3 minimum
    public GameObject fightNodePrefab;//make this into a list later, you need 3 minimum
    public GameObject bossNodePrefab;

    public GameObject[] fightNodePrefabs;

    public int oobTileLimit = 45;

    void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    public void RenderMap()
    {
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            for (int t = 0; t < mapGenerator.mapWidth; t++)
            {
                if (mapGenerator.mapArray[t, i] == 1)
                {
                    walkableTilemap.SetTile(new Vector3Int(t, i, 0), floorTile[Random.Range(0, floorTile.Length)]);
                }
                else
                {
                    walkableTilemap.SetTile(new Vector3Int(t, i, 0), darkGrassTile[Random.Range(0, darkGrassTile.Length)]);
                }
            }
        }
        RenderCorridorPath();
        RenderCorridorObelisks();
        RenderObstacleGround();
        RenderRandomWalkableDecor();
        RenderRandomObstacleDecor();
        RenderEdgeDecor();
        RenderEdgeDecorObjects();
        RenderCorridorPath();
    }

    public void RenderCorridorObelisks()
    {
        List<Vector2> placedPositions = new List<Vector2>();
        int step = 35;

        for (int i = 0; i < mapGenerator.corridorCenterline.Count - 1; i += step)
        {
            Vector2Int pos = mapGenerator.corridorCenterline[i];
            Vector2Int next = mapGenerator.corridorCenterline[Mathf.Min(i + 1, mapGenerator.corridorCenterline.Count - 1)];
            Vector2 checkPos = new Vector2(pos.x, pos.y);
            bool insideRoom = false;
            bool tooClose = false;
            for (int r = 0; r < mapGenerator.rooms.Count; r++)
            {
                float dist = Vector2.Distance(checkPos, new Vector2(mapGenerator.rooms[r].centerX, mapGenerator.rooms[r].centerY));
                if (dist < mapGenerator.rooms[r].radius + 15)
                {
                    insideRoom = true;
                    break;
                }
            }
            if (insideRoom) continue;
            for (int s = 0; s < placedPositions.Count; s++)
            {
                if (Vector2.Distance(checkPos, placedPositions[s]) < 8)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            Vector2 dir = new Vector2(next.x - pos.x, next.y - pos.y).normalized;
            Vector2 perp = new Vector2(-dir.y, dir.x);
            if (Random.value > 0.5f) perp = -perp;

            Vector2 spawnPos = checkPos + perp * 5f;
            placedPositions.Add(checkPos);
            Instantiate(obeliskPrefabs[Random.Range(0, obeliskPrefabs.Length)], new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
        }
    }
    public void RenderChests(Vector2 cacheCenter)
    {
        int randomChest = Random.Range(0, chests.Count);
        Instantiate(chests[randomChest], cacheCenter, Quaternion.identity);
    }
    public void RenderCachePrefab(Vector2 cacheCenter)
    {
        Debug.Log("Rendering Caches");
        Instantiate(cachePrefab, cacheCenter, Quaternion.identity);
    }
    public GameObject RenderFightNodePrefab(Vector2 fightNodeCenter)
    {
        return Instantiate(fightNodePrefab, fightNodeCenter, Quaternion.identity);
    }

    public GameObject RenderBossNodePrefab(Vector2 bossNodeCenter)
    {
        return Instantiate(bossNodePrefab, bossNodeCenter, Quaternion.identity);
    }
    public void RenderObstacleGround()
    {
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            for (int t = 0; t < mapGenerator.mapWidth; t++)
            {
                if (mapGenerator.mapArray[t, i] == 0)
                {
                    obstacleTilemap.SetTile(new Vector3Int(t, i, 0), obstacleTile);
                }
            }
        }
    }

    public void RenderRandomWalkableDecor()
    {
        for (int i = 1; i < mapGenerator.mapHeight - 1; i++)
        {
            for (int t = 1; t < mapGenerator.mapWidth - 1; t++)
            {
                if (mapGenerator.mapArray[t, i] == 1 && Random.Range(0, 101) < 4)
                {
                    walkableDecorTilemap.SetTile(new Vector3Int(t, i, 0), walkableDecorTile[Random.Range(0, walkableDecorTile.Length)]);
                }
            }
        }
    }

    public void RenderRandomObstacleDecor()
    {
        for (int i = 3; i < mapGenerator.mapHeight - 3; i++)
        {
            for (int t = 3; t < mapGenerator.mapWidth - 3; t++)
            {
                if (mapGenerator.mapArray[t, i] == 0 && Random.Range(0, 1001) < 800)
                {
                    walkableDecorTilemap.SetTile(new Vector3Int(t, i, 0), obstacleDecorTile2[Random.Range(0, obstacleDecorTile2.Length)]);
                }
                if (mapGenerator.mapArray[t, i] == 0 && Random.Range(0, 1001) < 25)
                {
                    bool tooClose = false;
                    for (int y = -5; y <= 5 && !tooClose; y++)
                    {
                        for (int x = -5; x <= 5 && !tooClose; x++)
                        {
                            int checkX = t + x;
                            int checkY = i + y;
                            if (checkX >= 0 && checkX < mapGenerator.mapWidth && checkY >= 0 && checkY < mapGenerator.mapHeight)
                            {
                                if (mapGenerator.mapArray[checkX, checkY] == 1)
                                    tooClose = true;
                            }
                        }
                    }
                    if (tooClose) continue;

                    float yOffset = Random.Range(0f, 0.01f);
                    Instantiate(obstacleDecorTile[Random.Range(0, obstacleDecorTile.Length)], new Vector3(t, i + yOffset, 0), Quaternion.identity);
                }
            }
        }
    }
    public void RenderEdgeDecor()
    {
        for (int i = 3; i < mapGenerator.mapHeight - 3; i++)
        {
            for (int t = 3; t < mapGenerator.mapWidth - 3; t++)
            {
                if (mapGenerator.mapArray[t, i] == 0 && Random.Range(0, 101) < 80)
                {
                    bool nearWalkable = false;
                    for (int y = -3; y <= 3 && !nearWalkable; y++)
                    {
                        for (int x = -3; x <= 3 && !nearWalkable; x++)
                        {
                            int checkX = t + x;
                            int checkY = i + y;
                            if (checkX >= 0 && checkX < mapGenerator.mapWidth && checkY >= 0 && checkY < mapGenerator.mapHeight)
                            {
                                if (mapGenerator.mapArray[checkX, checkY] == 1)
                                    nearWalkable = true;
                            }
                        }
                    }
                    if (!nearWalkable) continue;

                    walkableDecorTilemap.SetTile(new Vector3Int(t, i, 0), obstacleDecorTile3[Random.Range(0, obstacleDecorTile3.Length)]);
                }
            }
        }
    }
    public TileBase[] cobbleTiles;

    public void RenderCorridorPath()
    {
        foreach (Vector2Int pos in mapGenerator.corridorTiles)
        {
            cobbleTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), cobbleRuleTile);
        }
    }
    public void RenderEdges()
    {
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(0, i, 0), leftEdge);
            obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, i, 0), rightEdge);
        }
        for (int i = 0; i < mapGenerator.mapWidth; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(i, 0, 0), downEdge);
            obstacleTilemap.SetTile(new Vector3Int(i, mapGenerator.mapHeight, 0), upEdge);
        }
        obstacleTilemap.SetTile(new Vector3Int(0, 0, 0), bottomLeftEdge);
        obstacleTilemap.SetTile(new Vector3Int(0, mapGenerator.mapHeight, 0), topLeftEdge);
        obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, mapGenerator.mapHeight, 0), topRightEdge);
        obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, 0, 0), bottomRightEdge);
    }

    public void RenderWaterOutOfBounds()
    {
        for (int i = -oobTileLimit; i < mapGenerator.mapHeight + oobTileLimit; i++)
        {
            for (int t = -oobTileLimit; t < mapGenerator.mapWidth + oobTileLimit + 1; t++)
            {
                if (t < 1 || t > mapGenerator.mapWidth - 1 || i < 1 || i > mapGenerator.mapHeight - 1)
                {
                    oobTileMap.SetTile(new Vector3Int(t, i, 0), oobTile);
                }
            }
        }
    }


    public void RenderEdgeDecorObjects()
    {
        List<Vector2> spawnedPositions = new List<Vector2>();

        for (int i = 3; i < mapGenerator.mapHeight - 3; i++)
        {
            for (int t = 3; t < mapGenerator.mapWidth - 3; t++)
            {
                if (mapGenerator.mapArray[t, i] == 0 && Random.Range(0, 101) < 80)
                {
                    bool nearWalkable = false;
                    bool tooClose = false;

                    for (int y = -1; y <= 1 && !tooClose; y++)
                    {
                        for (int x = -2; x <= 2 && !tooClose; x++)
                        {
                            int checkX = t + x;
                            int checkY = i + y;
                            if (checkX >= 0 && checkX < mapGenerator.mapWidth && checkY >= 0 && checkY < mapGenerator.mapHeight)
                            {
                                if (mapGenerator.mapArray[checkX, checkY] == 1)
                                    tooClose = true;
                            }
                        }
                    }
                    if (tooClose) continue;

                    for (int y = -10; y <= 10 && !nearWalkable; y++)
                    {
                        for (int x = -10; x <= 10 && !nearWalkable; x++)
                        {
                            int checkX = t + x;
                            int checkY = i + y;
                            if (checkX >= 0 && checkX < mapGenerator.mapWidth && checkY >= 0 && checkY < mapGenerator.mapHeight)
                            {
                                if (mapGenerator.mapArray[checkX, checkY] == 1)
                                    nearWalkable = true;
                            }
                        }
                    }
                    if (!nearWalkable) continue;

                    // Check distance from other spawned objects
                    Vector2 thisPos = new Vector2(t, i);
                    bool tooCloseToOther = false;
                    for (int s = 0; s < spawnedPositions.Count; s++)
                    {
                        if (Vector2.Distance(thisPos, spawnedPositions[s]) < 3)
                        {
                            tooCloseToOther = true;
                            break;
                        }
                    }
                    if (tooCloseToOther) continue;

                    spawnedPositions.Add(thisPos);
                    float yOffset = Random.Range(0f, 0.01f);
                    Instantiate(edgeDecorObjects[Random.Range(0, edgeDecorObjects.Length)], new Vector3(t, i + yOffset, 0), Quaternion.identity);
                }
            }
        }
    }
    public void ViableEnemySpawns()
    {
        spawnManager.viableSpawnCenters = new int[mapGenerator.mapWidth, mapGenerator.mapHeight];
        for (int i = 3; i < mapGenerator.mapHeight - 3; i++)
        {
            for (int t = 3; t < mapGenerator.mapWidth - 3; t++)
            {
                if (mapGenerator.mapArray[t, i] == 1)
                {
                    if (mapGenerator.mapArray[t - 3, i + 3] == 1 && mapGenerator.mapArray[t, i + 3] == 1 &&
                        mapGenerator.mapArray[t + 3, i + 3] == 1 && mapGenerator.mapArray[t - 3, i] == 1 &&
                        mapGenerator.mapArray[t + 3, i] == 1 && mapGenerator.mapArray[t - 3, i - 3] == 1 &&
                        mapGenerator.mapArray[t, i - 3] == 1 && mapGenerator.mapArray[t + 3, i - 3] == 1)
                    {
                        spawnManager.viableSpawnCenters[t, i] = 2;
                    }
                }
            }
        }
    }
}