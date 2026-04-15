using UnityEngine;
using System.Collections.Generic;

public class RERManager : MonoBehaviour
{
    private MapGenerator mapGenerator;
    public GameObject rerSacrifice;
    public GameObject rerGamble;
    public GameObject rerCurse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
    }
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RERSelection()
    {
        for (int i = 0; i < mapGenerator.rERList.Count; i++)
        {
            int randomRoom = Random.Range(0, 3);
            switch(randomRoom)
            {
                case 0:
                    Instantiate(rerSacrifice, new Vector2(mapGenerator.rERList[i].x, mapGenerator.rERList[i].y),Quaternion.identity);
                    break;
                case 1:
                    Instantiate(rerGamble, new Vector2(mapGenerator.rERList[i].x, mapGenerator.rERList[i].y), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(rerCurse, new Vector2(mapGenerator.rERList[i].x, mapGenerator.rERList[i].y), Quaternion.identity);
                    break;

            }
        }
    }
}
