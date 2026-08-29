using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponLoadout
{
    public string loadoutName;
    [TextArea] public string description;
    public List<WeaponData> weapons = new List<WeaponData>();
}