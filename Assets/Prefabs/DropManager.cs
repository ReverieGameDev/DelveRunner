using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollDropTable(DropTableData dropTableData, Vector2 enemyPosition)
    {
        if (dropTableData == null) return;
        int totalWeight = 0;
        int randomDropWeight;
        DropEntry dropEntryWinner = null;
        int dropCount = dropTableData.rollCount;
        foreach (DropEntry dataPoint in dropTableData.entries)
        {
            if (dataPoint.guaranteedDrop)
            {
                for (int i = 0; i < dataPoint.itemCount; i++)
                {
                    
                    DropItem(enemyPosition, dataPoint.prefab, dataPoint.itemExplosionForce);
                }
            }
            if (!dataPoint.guaranteedDrop)
            totalWeight += dataPoint.weight;
        }
        for (int i = 0; i < dropCount; i++)
        {
            randomDropWeight = Random.Range(0, totalWeight);
            foreach (DropEntry drop in dropTableData.entries)
            {
                if (!drop.guaranteedDrop)
                {
                    randomDropWeight -= drop.weight;
                    if (randomDropWeight < 0)
                    {
                        dropEntryWinner = drop;
                        for (int x = 0; x < dropEntryWinner.itemCount; x++)
                        {
                            DropItem(enemyPosition, dropEntryWinner.prefab, drop.itemExplosionForce);
                        }
                        break;
                    }
                }
            }
        }
    }

    public void DropItem(Vector2 dropPos, GameObject drop, float explosionForce) 
    {
        GameObject droppedItem = Instantiate(drop, dropPos,Quaternion.identity);
        droppedItem.GetComponent<ItemDropAnim>().DropItemAnim(explosionForce);
    }
}
