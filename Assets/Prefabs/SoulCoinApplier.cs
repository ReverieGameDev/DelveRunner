using System.Collections.Generic;
using UnityEngine;

public class SoulCoinApplier : MonoBehaviour
{
    public List<SoulCoinNode> allNodes = new List<SoulCoinNode>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadSoulCoinData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadSoulCoinData()
    {
        string json = PlayerPrefs.GetString("SoulSave", "");
        PlayerCombat.Instance.ResetStatBonuses();
        if (json == "") return;
        SoulSaveData data = JsonUtility.FromJson<SoulSaveData>(json);
        PlayerCombat.Instance.soulCoins = data.soulCoins;

        for (int i = 0; i< allNodes.Count; i++)
        {
            for (int x = 0; x < data.ownedIds.Count; x++)
            {
                if (allNodes[i].id == data.ownedIds[x])
                {
                    allNodes[i].Apply(PlayerCombat.Instance, data.ownedValues[x]);
                    Debug.Log("Applied: " + allNodes[i].id);
                }
            }
        }
    }
}
