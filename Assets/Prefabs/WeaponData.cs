using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public Sprite wIcon;
    public string wName;
    public GameObject wProjectilePrefab;
    public GameObject wChargeProjectilePrefab;
    public float wDamage;
    public float wAS;
    public float wRange;
    public float wSwitchCD;
    public WeaponTargetingType weaponTargetingType;
    public WeaponStatusEffect weaponStatusEffect;
    public float wProcChance;
    public int wProjectileCount;
    public bool hasChargeAttack;
    public float chargeTime;
    public string wSpecialEffectDescription;
    public int wCost;
    public float wChargeTime;

}
public enum WeaponStatusEffect { None, Freeze, Burn, Stun, Enfeeble }
public enum WeaponTargetingType
{
    Single,
    NearestEnemy,
    Cone,
    Homing,
    Line
}