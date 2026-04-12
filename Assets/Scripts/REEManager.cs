using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REEManager : MonoBehaviour
{
    public List<Vector2> availableSpawnPoints = new List<Vector2>();
    public List<Vector2> activeSpawnPoints = new List<Vector2>();
    public GameObject encounterPrefab;

    void Start()
    {
        StartCoroutine("InitializeSpawnPoints");
    }

    IEnumerator InitializeSpawnPoints()
    {
        yield return null;
        MapGenerator mapGen = FindFirstObjectByType<MapGenerator>();
        availableSpawnPoints = new List<Vector2>(mapGen.corridorMidpoints);
        SelectAndSpawnEncounters();
    }

    private void SelectAndSpawnEncounters()
    {
        MapGenerator mapGen = FindFirstObjectByType<MapGenerator>();
        availableSpawnPoints = new List<Vector2>(mapGen.corridorMidpoints);
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            activeSpawnPoints.Add(availableSpawnPoints[randomIndex]);
            availableSpawnPoints.RemoveAt(randomIndex);
        }
        for (int i = 0; i < activeSpawnPoints.Count; i++)
        {
            Instantiate(encounterPrefab, activeSpawnPoints[i], Quaternion.identity);
        }
        activeSpawnPoints.Clear();
        availableSpawnPoints.Clear();
    }
}