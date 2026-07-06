using System.Collections.Generic;

[System.Serializable]
public class SoulSaveData
{
    public int soulCoins;
    public List<string> ownedIds = new List<string>();
    public List<int> ownedValues = new List<int>();
    public List<string> unlocked = new List<string>();
}