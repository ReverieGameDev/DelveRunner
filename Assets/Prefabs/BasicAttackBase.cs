using UnityEngine;

public abstract class BasicAttackBase : MonoBehaviour
{
    public abstract void FireBaseAttack(WeaponData weaponData, Vector2 direction);
}
