using System.Collections.Generic;
using UnityEngine;

public abstract class AugmentData : ScriptableObject
{
    public string augmentId;
    public int maxAugmentLevel;
    public Sprite augmentIcon;
    public int augmentTier = 1;
    public int augmentWeight = 0;
    public WeaponData requiredAugmentWeapon;
    public SoulCoinNode requiredAugmentSoulCoinUpgrade;
    [TextArea] public string augmentDescriptionName;
    [TextArea] public string augmentDescription;
    [TextArea] public string augmentPerLevelDescription;
    public abstract void Apply(PlayerCombat player, int currentLevel);
}