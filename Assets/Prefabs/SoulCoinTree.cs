using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Soul Coins/Tree")]
public class SoulCoinTree : ScriptableObject
{
    public string treeName;
    public List<SoulCoinNode> nodes;   // the 6, in slot order
}