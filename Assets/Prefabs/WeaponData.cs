using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public Sprite wIcon;
    public string wName;
    public string wSpecialEffectDescription;
    public int wCost;

    [Header("Normal Attack")]
    public GameObject wProjectilePrefab;
    public float wDamage;
    public float wAS;
    public float wRange;
    public float wProjectileSpeed;
    public float wProcChance;

    [Header("Normal Status Effect")]
    public int wStatusEffectDamage;
    public float wStatusEffectTickRate;
    public float wStatusEffectDuration;
    public float wStatusEffectPercentage;

    [Header("Charged Attack")]
    public bool hasChargeAttack;
    public float wChargeTime;
    public GameObject wChargeProjectilePrefab;
    public float wChargeProcChance;

    [Header("Charged Status Effect")]
    public int wChargeEffectDamage;
    public float wChargeEffectTickRate;
    public float wChargeEffectDuration;
}

public enum WeaponStatusEffect { None, Poison, Burn, Stun, Enfeeble, Cinder, Shock }
public enum WeaponTargetingType { Single, NearestEnemy, Cone, Homing, Line }