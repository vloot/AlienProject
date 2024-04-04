using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    public WeaponStats weaponStats;

    [SerializeField] protected LayerMask destructionLayerMask;

    protected float _timeSinceLastBullet;

    public abstract void Use(Vector3 position, Vector3 forward, Quaternion rotation);
}
