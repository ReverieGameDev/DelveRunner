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
    public List<SoulCoinNode> requires = new List<SoulCoinNode>();
    public List<SoulCoinNode> siblings = new List<SoulCoinNode>();
    [TextArea] public string description;
    [TextArea] public string abilityDetails;
    [TextArea] public string levelCost;
    public abstract void Apply(PlayerCombat player, int currentLevel);
}