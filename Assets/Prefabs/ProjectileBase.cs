using UnityEngine;

public abstract class BasicAttackBase : MonoBehaviour
{
    public abstract void WeaponInit(WeaponData weaponData, Vector2 direction);
}
