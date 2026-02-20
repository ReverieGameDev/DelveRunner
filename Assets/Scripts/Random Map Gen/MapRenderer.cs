using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private SpawnManager spawnManager;
    public Tilemap walkableTilemap;
    public Tilemap obstacleTilemap;
    public Tilemap walkableDecorTilemap;
    public Tilemap belowFloor;
    public TileBase[] floorTile;
    public TileBase[] walkableDecorTile;
    public Tilemap outOfBounds;
    public Tilemap obstacleDecorTilemap;
    public GameObject[] obstacleDecorTile;
    public GameObject[] obstacleDecorTile2;
    public GameObject[] belowFloorTile;
    public Tilemap oobTileMap;
    public TileBase oobTile;
    public TileBase obstacleTile;
    public TileBase leftEdge;
    public TileBase downEdge;
    public TileBase rightEdge;
    public TileBase upEdge;
    public TileBase bottomLeftEdge;
    public TileBase topLeftEdge;
    public TileBase bottomRightEdge;
    public TileBase topRightEdge;
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
                // Grass everywhere
                walkableTilemap.SetTile(new Vector3Int(t, i, 0), floorTile[UnityEngine.Random.Range(0, floorTile.Length)]);
            }
        }
        RenderRandomWalkableDecor();
        RenderRandomObstacleDecor();
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

    public void RenderRandomWalkableDecor()
    {
        for (int i = 1; i < mapGenerator.mapHeight - 1; i++)
        {
            for (int t = 1; t < mapGenerator.mapWidth - 1; t++)
            {
                int rngIfDecorShouldBePlaced = UnityEngine.Random.Range(0, 101);
                if (mapGenerator.mapArray[t, i] == 1 && rngIfDecorShouldBePlaced < 10)
                {
                    walkableDecorTilemap.SetTile(new Vector3Int(t, i, 0), walkableDecorTile[UnityEngine.Random.Range(0, walkableDecorTile.Length)]);
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
                GameObject[] chosenObstacleArray;
                int rngDecorArrayDecider = UnityEngine.Random.Range(1, 11);
                if (rngDecorArrayDecider < 11)
                {
                    chosenObstacleArray = obstacleDecorTile;
                }
                else
                {
                    chosenObstacleArray = obstacleDecorTile2;
                }
                int rngObstacleDecider = UnityEngine.Random.Range(0, chosenObstacleArray.Length);
                int rngIfObstacleShouldBePlaced = UnityEngine.Random.Range(0, 1001);
                if (mapGenerator.mapArray[t, i] == 0 && rngIfObstacleShouldBePlaced < 200)
                {
                    float yOffset = UnityEngine.Random.Range(0f, 0.01f);
                    Instantiate(chosenObstacleArray[rngObstacleDecider], new Vector3(t, i + yOffset, 0), Quaternion.identity);
                }
            }
        }
    }

    public void RenderEdges()
    {
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(0, i, 0), leftEdge);
        }
        for (int i = 0; i < mapGenerator.mapWidth; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(i, 0, 0), downEdge);
        }
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, i, 0), rightEdge);
        }
        for (int i = 0; i < mapGenerator.mapWidth; i++)
        {
            obstacleTilemap.SetTile(new Vector3Int(i, mapGenerator.mapHeight, 0), upEdge);
        }
        obstacleTilemap.SetTile(new Vector3Int(0, 0, 0), bottomLeftEdge);
        obstacleTilemap.SetTile(new Vector3Int(0, mapGenerator.mapHeight, 0), topLeftEdge);
        obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, mapGenerator.mapHeight, 0), topRightEdge);
        obstacleTilemap.SetTile(new Vector3Int(mapGenerator.mapWidth, 0, 0), bottomRightEdge);
    }

    public void RenderBelowGround()
    {
        for (int i = 0; i < mapGenerator.mapHeight; i++)
        {
            for (int t = 0; t < mapGenerator.mapWidth; t++)
            {
                if (mapGenerator.mapArray[t, i] == 0)
                {
                    int randomWaterTile = UnityEngine.Random.Range(0, belowFloorTile.Length);
                    GameObject water = Instantiate(belowFloorTile[randomWaterTile], new Vector3Int(t, i, 0), Quaternion.identity, transform);
                    Animator anim = water.GetComponent<Animator>();
                    if (anim != null && anim.runtimeAnimatorController != null)
                    {
                        anim.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
                    }
                }
            }
        }
        RenderWaterOutOfBounds();
    }
    public void RenderWaterOutOfBounds()
    {
        for (int i = -oobTileLimit; i < mapGenerator.mapHeight+ oobTileLimit; i++)
        {
            for (int t = -oobTileLimit; t < mapGenerator.mapWidth + oobTileLimit + 1; t++)
            {
                if(t <1 || t> mapGenerator.mapWidth-1)
                {
                    
                    oobTileMap.SetTile(new Vector3Int(t, i, 0), oobTile);
                }
                else if(i < 1 || i > mapGenerator.mapHeight-1)
                    {
                        
                        oobTileMap.SetTile(new Vector3Int(t, i, 0), oobTile);
                    }

            }
        }
    }
}