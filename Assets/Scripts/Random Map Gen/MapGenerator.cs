using UnityEngine;

public class Room
{
    public int centerX;
    public int centerY;
    public int radius;
    public string roomType; // "fightNode", "cache", "hidden", "spawn", "boss"
}
public class MapGenerator : MonoBehaviour
{
    public int mapHeight = 300;
    public int mapWidth = 300;
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
        mapArray = new int[mapWidth, mapHeight];

        for (int i = 0; i < mapHeight; i++)
        {
            for (int t = 0; t < mapWidth; t++)
            {
                mapArray[t, i] = 0;
            }
        }

        CarveRoom(150, 150, 30);

        mapRenderer.RenderMap();
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