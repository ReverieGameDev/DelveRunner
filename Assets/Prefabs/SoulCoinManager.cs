using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulCoinManager : MonoBehaviour
{
    public int soulCoins;
    public List<SoulCoinNode> allNodes;                              // the 36, dragged in via inspector
    public Dictionary<string, int> ownedLevels = new Dictionary<string, int>();
    HashSet<string> unlockedNodes;             // id is present = unlocked (NOT refundable)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetLevel(string id)
    {
        ownedLevels.TryGetValue(id, out int level);
        return level;
    }

    public void Buy(SoulCoinNode node)
    {
        int level = GetLevel(node.id);
        if (level >= node.maxLevel)
        {
            return;
        }
        if (soulCoins < node.cost[level])
        {
            return;
        }
        if (node.requires != null  )
        {
            if (GetLevel(node.requires.id) != node.requires.maxLevel)
            {
                return;
            }
        }
        if (unlockedNodes.Contains(node.id)) return;
        soulCoins -= node.cost[level];
        ownedLevels[node.id] = level + 1;
    }
}
