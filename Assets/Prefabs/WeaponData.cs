using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public Sprite wIcon;
    public string wName;
    public string wSpecialEffectDescription;
    public int wCost;

    [Header("Targeting")]
    public WeaponTargetingType weaponTargetingType;
    public float wRange;

    [Header("Normal Attack")]
    public GameObject wProjectilePrefab;
    public float wDamage;
    public float wAS;
    public float wSwitchCD;
    public int wProjectileCount;
    public float wProjectileSpeed;
    public WeaponStatusEffect weaponStatusEffect;
    public float wProcChance;

    [Header("Normal AOE")]
    public bool wHasAOE;
    public float wAOERadius;
    public float wAOEDamage;

    [Header("Charged Attack")]
    public bool hasChargeAttack;
    public float wChargeTime;
    public GameObject wChargeProjectilePrefab;
    public float wChargeDamage;
    public int wChargeProjectileCount;
    public float wChargeProjectileSpeed;
    public WeaponStatusEffect wChargeStatusEffect;
    public float wChargeProcChance;
    public float wStatusEffectDuration;
    public float wStatusEffectPercentage;

    [Header("Charged AOE")]
    public bool wChargeHasAOE;
    public float wChargeAOERadius;
    public float wChargeAOEDamage;
}

public enum WeaponStatusEffect { None, Poison, Burn, Stun, Enfeeble }

public enum WeaponTargetingType { Single, NearestEnemy, Cone, Homing, Line }