using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REEManager : MonoBehaviour
{
    public List<Vector2> reePoints = new List<Vector2>();
    public List<Vector2> filteredREEPoints = new List<Vector2>();
    public GameObject REEPoint;

    void Start()
    {
        StartCoroutine(DelayedSetup());
    }

    IEnumerator DelayedSetup()
    {
        yield return null; // wait one frame
        MapGenerator mapGen = FindFirstObjectByType<MapGenerator>();
        reePoints = new List<Vector2>(mapGen.corridorMidpoints);
        FilterAndSpawnREEs();
    }

    private void FilterAndSpawnREEs()
    {
        for (int i = 0; i < 4; i++)
        {
            int pointAdd = Random.Range(0, reePoints.Count);
            filteredREEPoints.Add(reePoints[pointAdd]);
            reePoints.RemoveAt(pointAdd);
        }
        for (int i = 0; i < filteredREEPoints.Count; i++)
        {
            Instantiate(REEPoint, filteredREEPoints[i], Quaternion.identity);
        }
    }
}