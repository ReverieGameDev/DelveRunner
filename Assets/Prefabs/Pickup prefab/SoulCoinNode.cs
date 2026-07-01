using UnityEngine;

public abstract class SoulCoinNode : ScriptableObject
{
    public string id;
    public string nodeName;
    public int maxLevel;
    public Sprite icon;
    public abstract void Apply(PlayerCombat player, int currentLevel);
}