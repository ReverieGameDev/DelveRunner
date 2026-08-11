using UnityEngine;

public abstract class ChargeAttackBase : MonoBehaviour
{
    public abstract void FireChargeAttack(WeaponData weaponData, float secondsHeld, float secondsToMaxCharge);
}
