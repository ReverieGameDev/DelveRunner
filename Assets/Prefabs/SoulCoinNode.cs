using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoulCoinNode : ScriptableObject
{
    public string id;
    public string nodeName;
    public int maxLevel;
    public Sprite icon;
    public int unlockCost;
    public List<int> cost = new List<int>();
    public SoulCoinNode requires;
    public abstract void Apply(PlayerCombat player, int currentLevel);
}