using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject prefab;
    public int weight;
    public bool guaranteedDrop;
    public int maxRolls = 99;
    public int itemCount = 1;
}

[CreateAssetMenu]
public class DropTableData : ScriptableObject
{
    public List<DropEntry> entries = new List<DropEntry>();
    public int rollCount = 1;
}

