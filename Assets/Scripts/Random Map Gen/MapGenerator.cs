using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapHeight = 100;
    public int mapWidth = 100;
    private float perlinNoiseObstacle = 0.2f;
    public int[,] mapArray;
    private MapRenderer mapRenderer;
    private float offsetX;
    private float offsetY;

    void Start()
    {
        mapRenderer = FindFirstObjectByType<MapRenderer>();
        PopulateMap();
    }

    public void PopulateMap()
    {
        offsetY = Random.Range(0f, 10000f);
        offsetX = Random.Range(0f, 10000f);
        mapArray = new int[mapWidth, mapHeight];

        // First noise pass
        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                float noiseValue = Mathf.PerlinNoise((t + offsetX) * 0.04f, (i + offsetY) * 0.04f);
                if (noiseValue <= perlinNoiseObstacle)
                {
                    mapArray[t, i] = 0; // obstacle
                }
                else
                {
                    mapArray[t, i] = 1; // floor
                }
            }
        }

        // Second noise pass — adds more holes
        float offsetX2 = Random.Range(0f, 10000f);
        float offsetY2 = Random.Range(0f, 10000f);
        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                float noiseValue = Mathf.PerlinNoise((t + offsetX2) * 0.03f, (i + offsetY2) * 0.03f);
                if (noiseValue <= 0.15f)
                {
                    mapArray[t, i] = 0; // obstacle
                }
            }
        }

        // Force floor buffer around edges
        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                if (t <= 3 || t >= mapWidth - 4 || i <= 3 || i >= mapHeight - 4)
                {
                    mapArray[t, i] = 1; // floor
                }
            }
        }

        RemoveUnwantedTiles();
        RemoveUnwantedTiles();

        // Render once at the end
        mapRenderer.RenderMap();
        mapRenderer.RenderBelowGround();
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