using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void WeaponInit(WeaponData weaponData, Vector2 direction);
}
